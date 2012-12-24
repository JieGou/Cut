#pragma once
#include "afxcmn.h"

#include <resource.h>

#include <dbsymtb.h>
#include <dbapserv.h>
#include <adslib.h>
#include <adui.h>
#include <acui.h>

// LineConfigDialog dialog

using namespace std;

namespace com
{

namespace guch
{

namespace assistent
{

namespace config
{

class LineConfigDialog : public CAcUiDialog
{
	DECLARE_DYNAMIC(LineConfigDialog)

public:
	LineConfigDialog(CWnd* pParent = NULL);   // standard constructor
	virtual ~LineConfigDialog();

// Dialog Data
	enum { IDD = IDD_DIALOG_LINE_CONFIG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL OnInitDialog();

	BOOL InitLineHeader();
	BOOL InitLineData();

	afx_msg void OnBnClickedButtonAdd();

	DECLARE_MESSAGE_MAP()

	BOOL InsertLine(const int index,
					const wstring& rID,
					const wstring& rName,
					const wstring& rKind,
					const wstring& rCategory,
					const wstring& rShape,
					const wstring& rSize,
					const wstring& rEffetSize,
					const wstring& rUnit,
					const wstring& rComment);

public:

	CAcUiListCtrl m_lineConfig;
};

} // end of config

} // end of assistant

} // end of guch

} // end of com