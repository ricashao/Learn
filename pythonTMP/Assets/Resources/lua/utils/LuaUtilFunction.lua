--LuaUtilFunction lua 工具方法集合

function table_leng(t)
  local leng=0
  for k, v in pairs(t) do
    leng=leng+1
  end
  return leng;
end

function table_rem(t,v)
   	for i=#t, 1, -1 do 
        if t[i] == v then 
            table.remove(t,i) 
            --print("table_rem = " .. i)
        end 
    end 
end

function table_print_pairs(t)
	for k, v in pairs(t) do
    	print(k,v);
end

function table_print_ipairs(t)
	for k, v in ipairs(t) do
    	print(k,v);
end