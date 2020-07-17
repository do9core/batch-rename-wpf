# batch-rename-wpf

批量更名工具(WPF)  
可以一次性更新很多文件的名称 ~~（是的就这么简单粗暴）~~

我在爬取一些图片后，发现其扩展名不正常  
于是写了这个玩意儿来批量修正扩展名

支持撤销操作、重做操作

***

This is a rename tool for windows users  

you can rename a lot of files in one time  

It supports undo and redo.

***

### 简单写一下制作的过程  

一开始设计的时候想法过于简单，觉得只要在原操作结果的基础上操作就可以了  
但是后来发现操作过程中的变化会导致整个生成结果混乱掉  
于是准备重新设计

有两个思路：

1. 用装饰器模式
2. 用操作栈

#### 未使用的思路：装饰器模式

很好理解，首先需要使用一个公共基类（比如说`RenameCommand`类)  
暴露一个公有方法(比如说`string Execute()`)，所有子类存储一个基类对象引用(相当于记录上次操作的结果)  
先使用不执行任何更改的一个子类对文件名进行包装（比如`DoNothingCommand`类)
然后每次追加修改，就额外进行一次包装
最后调用最外层的共有方法即得到修改后的新串

但是这么做有一些缺点：

* 不方便处理撤销，我认为撤销其实是一个很重要的操作
* 对每个文件名都需要进行包装，在批量处理时候可能会创建大量对象，耗费内存，性能必然不佳

最终没有采取这种方案实现

#### 最终的实现方案：操作栈

也不是很困难，同样使用一个公共基类（`RenameCommand`）  
然后使用一个操作栈记录这些操作

这个栈并不是严格意义上的栈，允许对其进行从栈底到栈顶的遍历  
我们只是在进行追加/撤销操作时才按栈的方式对其操作

每当用户追加一个新的修改，就向栈内压入一个子类操作对象  
撤销修改时，弹出一个子类操作对象（同理，重做功能则是再追加一个栈，记录这些被弹出的对象）

当我们需要获取新文件名时候，只需要从栈底开始，依次调用公有方法（`string Execute(string input)`）  
即可取得需要的新文件名

相比上面的方式，这种方式就解决了撤销和内存消耗的问题

~~看了看代码又想了想，这™不就是命令模式吗，所以把Operation都改成了Command~~

### LICENSE

> Copyright 2020 do9core
>
> Licensed under the Apache License, Version 2.0 (the "License");
> you may not use this file except in compliance with the License.
> You may obtain a copy of the License at
>
>     http://www.apache.org/licenses/LICENSE-2.0
>
> Unless required by applicable law or agreed to in writing, software
> distributed under the License is distributed on an "AS IS" BASIS,
> WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
> See the License for the specific language governing permissions and
> limitations under the License.
