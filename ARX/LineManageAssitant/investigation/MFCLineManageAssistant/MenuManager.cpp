
#include "MenuLMAMain.h"

#include "MenuManager.h"
#include "aced.h"

#include "StdAfx.h"

MenuManager* MenuManager::gMenuManager = NULL;

void MenuManager::CreateMenu(void* appId)
{
	if( gMenuManager == NULL )
	{
		gMenuManager = new MenuManager(appId);
	}
}

MenuManager* MenuManager::instance()
{
	assert(gMenuManager);

	return gMenuManager;
}

void MenuManager::unRegister()
{
	if( gMenuManager )
	{
		delete gMenuManager;
	}
}

MenuManager::MenuManager(const void* appId)
	:mAppId(appId)
{
	RegisterMenu();
}

MenuManager::~MenuManager(void)
{
	UnRegisterMenu();
}

void MenuManager::RegisterMenu()
{
	mpMainMenu = new MenuLMAMain();
	acedAddDefaultContextMenu(mpMainMenu,mAppId,L"管线辅助系统");

	acutPrintf(L"菜单加载成功\n");
}

void MenuManager::UnRegisterMenu()
{
	if( mpMainMenu )
	{
		acutPrintf(L"辅助系统菜单开始卸载\n");
		acedRemoveDefaultContextMenu(mpMainMenu ); // 移除默认上下文菜单
	}

	acutPrintf(L"菜单卸载成功\n");
}
