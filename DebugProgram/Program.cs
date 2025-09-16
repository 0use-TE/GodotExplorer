using GodotBehaviorTree;
using GodotBehaviorTree.Core;


var _tree = BehaviorTree.CreateTree();
_tree.BuildTree()
    .Selector()
        .Sequence()
            .Condition((delta) => true)
            //跟随玩家
            .Action((delta) =>
            {
                Console.WriteLine("follow player");
                return NodeState.Success;
            })
    .End()
        .Action((delta) =>
        {
            Console.WriteLine("idle");
            return NodeState.Success;
        });


//Run
while (true)
{
    _tree.Tick(1);
    Thread.Sleep(1000);
}