#pragma once

#include <resource.h>
#include <dbsymtb.h>
#include <dbapserv.h>
#include <adslib.h>
#include <adui.h>
#include <acui.h>

// AddLineDialog dialog

class AddLineDialog : public CAcUiDialog
{
	DECLARE_DYNAMIC(AddLineDialog)

public:
	AddLineDialog(CWnd* pParent = NULL);   // standard constructor
	virtual ~AddLineDialog();

// Dialog Data
	enum { IDD = IDD_DIALOG_LINE_ADD };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
};
