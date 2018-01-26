MailGlobalEntry={};

--local MailPanelCtrl = require 'lua/modules/Lobby/modules/Mail/MailPanelCtrl'

function MailGlobalEntry.OnMailTabSelected(indexd, transform, data)
	print("OnMailTabSelected:".. tostring(indexd));


	local select1 = transform:Find('Select1');
	local select2 = transform:Find('Select2');
	select2.gameObject:SetActive(true);
	select1.gameObject:SetActive(false);


	if(EventExist("Global2Ctrl_MailModules_TheTabSelected")) then
		es("Global2Ctrl_MailModules_TheTabSelected",{});
	else
		print("Global2Ctrl_MailModules_TabSelected not exist");
	end




end

function MailGlobalEntry.OnMailTabUnSelected(indexd, transform, data)
	print("OnMailTabUnSelected:".. tostring(indexd));
	local select1 = transform:Find('Select1');
	local select2 = transform:Find('Select2');
	select2.gameObject:SetActive(false);
	select1.gameObject:SetActive(true);

	--if(EventExist("Global2Ctrl_MailModules_TabSelected")) then
	--	es("Global2Ctrl_MailModules_TabSelected","Service");
	--end

end


function MailGlobalEntry.OnDataRefrsh(index,transform,data)

	if(EventExist("Global2Ctrl_MailModules_SetMailItemContent"))then
		print("OnDataRefresh");

		print("Title:"..data.title);

		es("Global2Ctrl_MailModules_SetMailItemContent",{["Index"]=index,["Transform"]=transform,["Data"]=data});
	end



end

function MailGlobalEntry.OnSetSelectedState(transform,bselected)

	if(EventExist("Global2Ctrl_MailModules_SetMailItemSelected"))then
		es("Global2Ctrl_MailModules_SetMailItemSelected",{["Transform"]=transform,["BS"]=bselected});
	end

	--if bselected then
	--	print("Selcted State:True");
    --
	--else
	--	print("Selcted State:False");
	--end

end