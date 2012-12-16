#pragma once

#include <resource.h>
#include <dbsymtb.h>
#include <dbapserv.h>
#include <adslib.h>
#include <adui.h>
#include <acui.h>

class LMALineConfigManagerDialog :public CAcUiDialog
{
public:
	LMALineConfigManagerDialog(CWnd* parent = NULL);

	~LMALineConfigManagerDialog(void);

	 enum { IDD = IDD_DIALOG_LINE_CONFIG };
};

