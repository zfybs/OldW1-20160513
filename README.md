# OldW
老王课题组 Revit开发

在基坑开挖与施工过程中的监测数据的处理与报警。

# Git 分支管理
在实际开发中，我们应该按照几个基本原则进行分支管理：

1. 首先，master分支应该是非常稳定的，也就是仅用来发布新版本，平时不能在上面干活
2. 那在哪里干活呢？干活都在dev分支上，也就是说，dev分支是不稳定的，到某个时候，比如2.0版本发布时，再把dev分支合并到master上，在master分支发新版本
3. 你和你的小伙伴每个人都在dev分支上干活，每个人都有自己的分支，时不时往dev分支上合并就可以了
所以，团队合作的分支看起来就像这个样子： https://segmentfault.com/img/bVcc7H
