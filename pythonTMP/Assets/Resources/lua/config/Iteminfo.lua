Iteminfo ={
	{mid=1,name="金币",price=1,price_type=0,collect=0,amount_max=0,sort=1,rewards=""},
	{mid=2,name="钻石",price=1,price_type=0,collect=0,amount_max=0,sort=2,rewards=""},
	{mid=3,name="点券",price=1,price_type=0,collect=0,amount_max=0,sort=3,rewards=""},
	{mid=4,name="记牌器（1天）",price=15,price_type=2,collect=1,amount_max=1,sort=4,rewards="5,10001300,1"},
	{mid=5,name="记牌器（7天）",price=90,price_type=2,collect=1,amount_max=1,sort=5,rewards="5,10001300,7"},
	{mid=6,name="记牌器（30天）",price=30,price_type=2,collect=1,amount_max=1,sort=6,rewards="5,10001300,30"},
	{mid=7,name="记牌器（永久）",price=99,price_type=2,collect=1,amount_max=1,sort=7,rewards="5,10001300,0"},
	{mid=8,name="房卡（1张）",price=10,price_type=2,collect=1,amount_max=100,sort=8,rewards=""},
	{mid=9,name="房卡（10张）",price=100,price_type=2,collect=0,amount_max=0,sort=9,rewards="4,8,10"},
	{mid=10,name="房卡（30张）",price=300,price_type=2,collect=0,amount_max=0,sort=10,rewards="4,8,30"},
	{mid=11,name="房卡（50张）",price=500,price_type=2,collect=0,amount_max=0,sort=11,rewards="4,8,50"},
	{mid=12,name="36000金币",price=30,price_type=2,collect=0,amount_max=0,sort=12,rewards="1,36000"},
	{mid=13,name="75600金币",price=60,price_type=2,collect=0,amount_max=0,sort=13,rewards="1,75600"},
	{mid=14,name="132000金币",price=100,price_type=2,collect=0,amount_max=0,sort=14,rewards="1,132000"},
	{mid=15,name="276000金币",price=200,price_type=2,collect=0,amount_max=0,sort=15,rewards="1,276000"},
	{mid=16,name="720000金币",price=500,price_type=2,collect=0,amount_max=0,sort=16,rewards="1,720000"},
	{mid=17,name="1500000金币",price=1000,price_type=2,collect=0,amount_max=0,sort=17,rewards="1,1500000"},
	{mid=18,name="2600000金币",price=2000,price_type=2,collect=0,amount_max=0,sort=18,rewards="1,2600000"},
	{mid=19,name="6500000金币",price=5000,price_type=2,collect=0,amount_max=0,sort=19,rewards="1,6500000"},
	{mid=20,name="30钻石",price=3,price_type=4,collect=0,amount_max=0,sort=20,rewards="2,300"},
	{mid=21,name="60钻石",price=6,price_type=4,collect=0,amount_max=0,sort=21,rewards="2,60"},
	{mid=22,name="180钻石",price=18,price_type=4,collect=0,amount_max=0,sort=22,rewards="2,180"},
	{mid=23,name="300钻石",price=30,price_type=4,collect=0,amount_max=0,sort=23,rewards="2,300"},
	{mid=24,name="680钻石",price=68,price_type=4,collect=0,amount_max=0,sort=24,rewards="2,680"},
	{mid=25,name="1680钻石",price=168,price_type=4,collect=0,amount_max=0,sort=25,rewards="2,1680"},
	{mid=26,name="3280钻石",price=328,price_type=4,collect=0,amount_max=0,sort=26,rewards="2,3280"},
	{mid=27,name="5000钻石",price=500,price_type=4,collect=0,amount_max=0,sort=27,rewards="2,5000"},
	{mid=28,name="电饭煲",price=100,price_type=3,collect=0,amount_max=0,sort=25,rewards="6,1680"},
	{mid=29,name="充气娃娃",price=250,price_type=3,collect=0,amount_max=0,sort=26,rewards="6,3280"},
	{mid=30,name="蛋白粉",price=300,price_type=3,collect=0,amount_max=0,sort=26,rewards="6,3280"}
}
--write by hand
GoodsConfigs = {}
GoodsConfigs.init = function()
	GoodsConfigs.itemData = Iteminfo
	GoodsConfigs.itemDataType = {}
	for k,v in pairs(Iteminfo) do
		if GoodsConfigs.itemDataType[v.price_type] == nil then GoodsConfigs.itemDataType[v.price_type] = {} end
		table.insert(GoodsConfigs.itemDataType[v.price_type],v)
	end
	Iteminfo = nil
end

 GoodsConfigs.getCfdsByRewardType = function(typek)
	local tb={}
	for _,v in pairs(GoodsConfigs.itemData) do
		local rewards = split(v.rewards,',')
		if rewards[1] == typek then
			table.insert(tb,v)
		end
	end
	return tb
 end


function GoodsConfigs.getItemsByType(itemtype)
	return GoodsConfigs.itemDataType[itemtype] or {}
end

function GoodsConfigs.getItemByID(mid)
	return GoodsConfigs.itemData[mid]
end

GoodsConfigs.init()

