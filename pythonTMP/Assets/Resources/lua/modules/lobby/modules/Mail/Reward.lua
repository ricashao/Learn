local Reward=
{
    ["ID"]=-1,
    ["Name"]="",
    ["Arg_1"]=-1,
    ["Arg_2"]=-1,
    ["Count"]=0,
    ["Path"]="",
};

function  Reward:New()

    local self = {}

    setmetatable( self , {__index = Reward})

    return self;
end

function Reward:SetID(nid)
    self.ID=nid;
end

function Reward:GetID()
    return self.ID;
end

function Reward:SetName(strName)
    self.Name=strName;
end

function Reward:GetName()
    return self.Name;
end

function Reward:SetCount(nCnt)
    self.Count=nCnt;
end

function Reward:GetCount()
    return self.Count;
end

function Reward:SetPath(strPath)
    self.Path=strPath;
end

function Reward:GetPath()
    return self.Path;
end

function Reward:SetRewardArg1(ag1)
    self.Arg_1=ag1;
end

function Reward:GetRewardArg1()
    return self.Arg_1;
end

function Reward:SetRewardArg2(ag2)
    self.Arg_2=ag2;
end

function Reward:GetRewardArg2()
    return self.Arg_2;
end

return Reward;
