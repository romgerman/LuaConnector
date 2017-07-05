/// <reference path="typings/types-gt-mp/index.d.ts" />

function Urlify(arr) {
	var result = "";

	arr.forEach((item, i) => {
		result += item;

		if (i !== arr.length)
			result += "/";
	});

	return result;
}

var MenuManager = {
	MenuHandlers: {},
	MenuPool: null,

	ProcessEvents: function (name, data) {
		var uri = name.split('/');

		if (uri.length <= 1)
			return;

		var handlerId = uri[0];

		if (MenuManager.MenuHandlers[handlerId]) {
			switch (uri[1]) {
				case "register":
					MenuManager.RegisterMenu(handlerId, uri[2], data);
					break;
				case "unregister":
					delete MenuManager.MenuHandlers[handlerId][uri[2]];
					break;
				case "open":
					MenuManager.OpenMenu(handlerId, uri[2]);
					break;
				case "close":
					MenuManager.CloseMenu(handlerId, uri[2]);
					break;
			}
		}
	},

	RegisterMenu: function(handler, name, def) {
		if (!def) return;

		var menu = MenuManager.FromJsonToMenu(def);

		menu.Menu.OnItemSelect.connect((item, index) => {
			API.triggerServerEvent(Urlify([ handler, "click", name, menu.ItemsId[index] ]));
		});

		MenuManager.MenuPool.Add(menu.Menu);
		MenuManager.MenuHandlers[handler][name] = menu;
	},

	FromJsonToMenu: function(json) {
		var def = JSON.parse(json);

		var menu = API.createMenu(def.title, def.subtitle, def.x, def.y, 6);
		var itemsId = { };
		
		def.items.forEach((item, index) => {
			var menuItem = API.createMenuItem(item.key, item.value);

			itemsId[index] = item.id;
			
			menu.AddItem(menuItem);
		});

		return { Menu: menu, ItemsId: itemsId };
	},

	OpenMenu: function(handler, name) {
		MenuManager.MenuHandlers[handler][name].Menu.Visible = true;
	},
	CloseMenu: function(handler, name) {
		MenuManager.MenuHandlers[handler][name].Menu.Visible = false;
	}
};

API.onResourceStart.connect(() => {
	MenuManager.MenuPool = API.getMenuPool();
});

API.onServerEventTrigger.connect((name, args) => {	
	if (name === "LUA_RegisterMenuHandler") {
		if (args[0]) MenuManager.MenuHandlers[args[0]] = { };
	} else {
		if (name) MenuManager.ProcessEvents(name, args.Length > 0 ? args[0] : null);
	}
});

API.onUpdate.connect(() => {
    if (MenuManager.MenuPool) {
        MenuManager.MenuPool.ProcessMenus();
    }
});
