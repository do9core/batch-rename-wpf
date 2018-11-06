using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using do9Rename.Helpers;
using do9Rename.Core;

namespace do9Rename.ViewModels
{
    class OptionVM : NotificationObject
    {
        public const string SUBSTRACT = "SUBSTRACT";     //截取功能
        public const string APPEND = "APPEND";           //插入功能
        public const string REPLACE = "REPLACE";         //替换功能
        public const string NOTHING = "NOTHING";         //啥也不干

        #region 私有变量

        private static readonly char[] INVALID_CHARS = @"\/:*?""<>|".ToCharArray();

        private ObservableOrderStack<RenameOperation> _operations;
        private ObservableOrderStack<RenameOperation> _undos;
        private RemoveExtOperation _removeExt;
        private AppendExtOperation _appendExt;

        private string _msg;
        private int _selectedIndex;
        private bool _withExt;

        private int _subSkip;
        private int _subTake;
        private bool _subHeadFirst;

        private int _appendSkip;
        private string _appendText;

        private string _replaceOld;
        private string _replaceNew;

        #endregion

        #region 界面绑定属性

        /// <summary>
        /// 影响扩展名否
        /// </summary>
        public bool WithExtension
        {
            get => _withExt;
            set => Set(ref _withExt, value);
        }

        /// <summary>
        /// 界面显示的消息
        /// </summary>
        public string Message
        {
            get => _msg;
            set => Set(ref _msg, value);
        }

        /// <summary>
        /// 文件列表选中项的索引
        /// </summary>
        public int SelectedIndex
        {
            get => _selectedIndex;
            set => Set(ref _selectedIndex, value);
        }

        /// <summary>
        /// 原文件名
        /// </summary>
        public ObservableCollection<FileInfo> OldNames { get; private set; }

        /// <summary>
        /// 新文件名
        /// </summary>
        public ObservableCollection<string> NewNames { get; private set; }

        //截取功能的参数

        /// <summary>
        /// 跳过的字符数
        /// </summary>
        public string SubSkip
        {
            get => $"{_subSkip}";
            set
            {
                if (!int.TryParse(value, out int temp))
                {
                    Message = "不合法的输入";
                    return;
                }
                Set(ref _subSkip, temp);
            }
        }

        /// <summary>
        /// 获取的字符数
        /// </summary>
        public string SubTake
        {
            get => $"{_subTake}";
            set
            {
                if (int.TryParse(value, out int temp))
                {
                    if (temp >= 0)
                    {
                        Set(ref _subTake, temp);
                        return;
                    }
                }
                Message = "不合法输入";
            }
        }

        /// <summary>
        /// 是否从前方开始计数
        /// </summary>
        public bool SubHeadFirst
        {
            get => _subHeadFirst;
            set => Set(ref _subHeadFirst, value);
        }

        //插入功能参数

        /// <summary>
        /// 插入位置
        /// </summary>
        public string AppendSkip
        {
            get => $"{_appendSkip}";
            set
            {
                if (int.TryParse(value, out int temp))
                {
                    if (temp >= 0)
                    {
                        Set(ref _appendSkip, temp);
                        return;
                    }
                }
                Message = "不合法的输入";
            }
        }

        /// <summary>
        /// 插入内容
        /// </summary>
        public string AppendText
        {
            get => _appendText;
            set => Set(ref _appendText, value);
        }

        //替换功能参数

        /// <summary>
        /// 要替换的内容
        /// </summary>
        public string ReplaceOld
        {
            get => _replaceOld;
            set => Set(ref _replaceOld, value);
        }

        /// <summary>
        /// 替换为的内容
        /// </summary>
        public string ReplaceNew
        {
            get => _replaceNew;
            set => Set(ref _replaceNew, value);
        }

        #endregion

        #region 界面绑定命令

        public ICommand AddOperationCommand { get; set; }
        public ICommand ExecuteCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand DeleteOneCommand { get; set; }
        public ICommand UndoCommand { get; set; }
        public ICommand RedoCommand { get; set; }
        public ICommand SelectFolderCommand { get; set; }
        public ICommand AddFileCommand { get; set; }
        public ICommand ReferenceCommand { get; set; }
        public ICommand CheckExtCommand { get; set; }
        public ICommand ClearOperationCommand { get; set; }

        #endregion

        public OptionVM()
        {
            // 初始化内部私有变量
            _operations = new ObservableOrderStack<RenameOperation>();
            _undos = new ObservableOrderStack<RenameOperation>();
            _removeExt = Singleton<RemoveExtOperation>.Instance;
            _appendExt = Singleton<AppendExtOperation>.Instance;

            // 初始化Command对象
            AddOperationCommand = new RelayCommand<string>(AddOperation);
            ExecuteCommand = new RelayCommand(ExecuteRename);
            RefreshCommand = new RelayCommand(RefreshOldName);
            ClearCommand = new RelayCommand(ClearAll);
            SelectFolderCommand = new RelayCommand(AddDirectory);
            AddFileCommand = new RelayCommand(AddFiles);
            CheckExtCommand = new RelayCommand(UpdateNewName);
            ClearOperationCommand = new RelayCommand(() => { _operations.Clear(); _undos.Clear(); UpdateNewName(); });
            ReferenceCommand = new RelayCommand(() => MessageBox.Show("请将问题原因发送到邮箱：do9core@outlook.com", "提示"));
            DeleteOneCommand = new RelayCommand(DeleteSelected, () => SelectedIndex >= 0);
            UndoCommand = new RelayCommand(Undo, () => _operations.Count > 0);
            RedoCommand = new RelayCommand(Redo, () => _undos.Count > 0);

            // 初始化绑定属性

            // 全局属性
            OldNames = new ObservableCollection<FileInfo>();
            NewNames = new ObservableCollection<string>();
            SelectedIndex = -1;
            WithExtension = true;

            // 截取功能属性
            SubSkip = "0";
            SubTake = "99";
            SubHeadFirst = true;

            // 插入功能属性
            AppendSkip = "0";
            AppendText = "";

            // 替换功能属性
            ReplaceOld = "a";
            ReplaceNew = "b";

            // 完毕
            Message = "就绪";
        }

        /// <summary>
        /// 向文件列表添加选中的文件
        /// </summary>
        private void AddFiles()
        {
            CommonOpenFileDialog cofd = new CommonOpenFileDialog
            {
                Multiselect = true,
                Title = "选择一个或多个文件"
            };

            if (cofd.ShowDialog() != CommonFileDialogResult.Ok)
            {
                Message = "没有选择文件";
                return;
            }

            int count = cofd.FileNames.Count();
            foreach (var fname in cofd.FileNames)
            {
                OldNames.Add(new FileInfo(fname));
            }

            UpdateNewName();
            Message = count > 1 ? $"已添加{count}个文件" : $"已添加文件{cofd.FileName}";
        }

        /// <summary>
        /// 向文件列表添加选中目录下的所有文件
        /// </summary>
        private void AddDirectory()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "选择一个文件夹"
            };

            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
            {
                Message = "没有加载目录";
                return;
            }

            DirectoryInfo dir = new DirectoryInfo(dialog.FileName);
            if (!dir.Exists)
            {
                Message = "目录不存在";
                return;
            }

            foreach (var fInf in dir.GetFiles())
            {
                OldNames.Add(fInf);
            }
            UpdateNewName();
            Message = $"已添加目录{dir.FullName}";
        }

        /// <summary>
        /// 获取操作后的新文件名
        /// </summary>
        /// <param name="fInf">文件信息</param>
        /// <returns>新文件名</returns>
        private string GetNewName(FileInfo fInf)
        {
            _removeExt.Extension = fInf.Extension;
            _appendExt.Extension = fInf.Extension;

            string result = WithExtension ? fInf.Name : _removeExt.Execute(fInf.Name);

            foreach (var operation in _operations)
            {
                result = operation.Execute(result);
            }

            result = WithExtension ? result : _appendExt.Execute(result);
            return result;
        }

        /// <summary>
        /// 更新列表显示的新文件名
        /// </summary>
        private void UpdateNewName()
        {
            NewNames.Clear();
            try
            {
                foreach (var fInf in OldNames)
                {
                    NewNames.Add(GetNewName(fInf));
                }
            }
            catch (ArgumentException e)
            {
                Message = e.Message;
                return;
            }
        }

        /// <summary>
        /// 刷新文件列表的旧文件名
        /// </summary>
        private void RefreshOldName()
        {
            var tempList = OldNames.ToList();
            OldNames.Clear();

            foreach (var fInf in tempList)
            {
                fInf.Refresh();
                OldNames.Add(fInf);
            }

            UpdateNewName();
            Message = "刷新完毕";
        }

        /// <summary>
        /// 向操作栈中追加一项
        /// </summary>
        /// <param name="operation">操作类型</param>
        private void AddOperation(string operation)
        {
            switch (operation)
            {
                case SUBSTRACT:
                    var sub = new SubstractOperation
                    {
                        Skip = _subSkip,
                        Take = _subTake,
                        HeadFirst = _subHeadFirst
                    };
                    _operations.Push(sub);
                    Message = $"追加操作 - {sub}";
                    break;
                case APPEND:
                    var append = new AppendOpreation
                    {
                        Skip = _appendSkip,
                        AppendText = _appendText
                    };
                    _operations.Push(append);
                    Message = $"追加操作 - {append}";
                    break;
                case REPLACE:
                    var replace = new ReplaceOpreation
                    {
                        OldText = _replaceOld,
                        NewText = _replaceNew
                    };
                    _operations.Push(replace);
                    Message = $"追加操作 - {replace}";
                    break;
                case NOTHING:
                    _operations.Push(new DoNothingOpreation());
                    break;
                default:
                    throw new ArgumentException();
            }
            UpdateNewName();
        }

        /// <summary>
        /// 执行命名，体现在文件上
        /// </summary>
        private void ExecuteRename()
        {
            int error = 0;
            for (int i = 0; i < OldNames.Count; ++i)
            {
                if (!IsValidFileName(NewNames[i]))
                {
                    ++error;
                    continue;
                }
                var dir = OldNames[i].Directory;
                OldNames[i].MoveTo($"{dir.FullName}/{NewNames[i]}");
            }
            RefreshOldName();
            UpdateNewName();

            _operations.Clear();
            _undos.Clear();

            Message = error > 0 ? $"{error}个文件重命名失败，已跳过" : "重命名成功完成";
        }

        /// <summary>
        /// 检查文件名中是否有不合法字符
        /// </summary>
        /// <param name="fname">文件名</param>
        /// <returns>是否合法</returns>
        private static bool IsValidFileName(string fname)
        {
            for (int i = 0; i < INVALID_CHARS.Length; ++i)
            {
                if (fname.Contains(INVALID_CHARS[i]))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 清空文件列表和操作栈
        /// </summary>
        private void ClearAll()
        {
            NewNames.Clear();
            OldNames.Clear();
            _operations.Clear();
            _undos.Clear();

            Message = "成功清除列表";
        }

        /// <summary>
        /// 从文件列表中移除选中项
        /// </summary>
        private void DeleteSelected()
        {
            int tempIndex = SelectedIndex;
            string tempName = OldNames[tempIndex].FullName;

            OldNames.RemoveAt(SelectedIndex);
            NewNames.RemoveAt(tempIndex);

            Message = $"已移除{tempName}";
        }

        /// <summary>
        /// 撤销操作
        /// </summary>
        private void Undo()
        {
            var lastOper = _operations.Pop();
            _undos.Push(lastOper);
            UpdateNewName();

            Message = $"已撤销操作 - {lastOper}";
        }

        /// <summary>
        /// 重做操作
        /// </summary>
        private void Redo()
        {
            var lastOper = _undos.Pop();
            _operations.Push(lastOper);
            UpdateNewName();

            Message = $"已重做操作 - {lastOper}";
        }
    }
}
