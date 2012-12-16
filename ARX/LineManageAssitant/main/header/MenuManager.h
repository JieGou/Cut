#pragma once

#include "MenuLMAMain.h"

class MenuManager
{
public:

	static void CreateMenu(void* appId);

	static MenuManager* instance();

	static void unRegister();

private:

	MenuManager(const void* appId);

	~MenuManager(void);

	static MenuManager* gMenuManager;

	const void* mAppId;

	MenuLMAMain* mpMainMenu;

	/**
	* 注册系统菜单
	**/
	void RegisterMenu();

	/**
	* 注销系统菜单
	**/
	void UnRegisterMenu();
};

