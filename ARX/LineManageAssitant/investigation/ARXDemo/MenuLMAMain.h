#pragma once
#include <afxwin.h>

#include "aced.h"

class MenuLMAMain :
	public AcEdUIContext
{

public:
	MenuLMAMain(void);
	~MenuLMAMain(void);

    virtual void* getMenuContext(const AcRxClass *pClass, const AcDbObjectIdArray& ids) ;
    virtual void  onCommand(Adesk::UInt32 cmdIndex);
    virtual void OnUpdateMenu();

private:
    CMenu *m_pMenu;
    HMENU m_tempHMenu;
};

