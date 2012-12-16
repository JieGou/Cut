#include "afxwin.h"

#pragma once
#include "aced.h"

class MenuLMAMain : public AcEdUIContext
{

public:
	MenuLMAMain(void);
	~MenuLMAMain(void);

    virtual void* getMenuContext(const AcRxClass *pClass, const AcDbObjectIdArray& ids) ;
    virtual void  onCommand(Adesk::UInt32 cmdIndex);
    virtual void OnUpdateMenu();

private:

	HMENU m_tempHMenu;
    CMenu *mpMenu;
};

