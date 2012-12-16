
#include "MenuLMAMain.h"
#include "resource.h"
#include <acdocman.h>

HINSTANCE _hdllInstance = NULL;//全局变量

MenuLMAMain::MenuLMAMain(void)
{
	acDocManagerPtr()->pushResourceHandle(_hdllInstance);
    m_pMenu = new CMenu;
    m_pMenu->LoadMenu(IDR_MENU1); // IDR _MENU1是要调入的菜单
    acDocManager->popResourceHandle();
}

MenuLMAMain::~MenuLMAMain(void)
{
	    if (m_pMenu) 
        delete m_pMenu;
}

void* MenuLMAMain::getMenuContext(const AcRxClass *, const AcDbObjectIdArray&)
{
   m_tempHMenu = m_pMenu->GetSubMenu(0)->GetSafeHmenu();   
   return &m_tempHMenu;
}

void MenuLMAMain::onCommand(Adesk::UInt32 cmdIndex)
{
    acDocManager->pushResourceHandle(_hdllInstance);

    CString str1;
    m_pMenu->GetMenuString(cmdIndex,str1,MF_BYCOMMAND);
    acedPostCommandPrompt();
    acDocManager->popResourceHandle();
}

void MenuLMAMain::OnUpdateMenu()
{
}

