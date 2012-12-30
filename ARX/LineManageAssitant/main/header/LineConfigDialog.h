#pragma once
#include "afxcmn.h"

#include <resource.h>

#include <dbsymtb.h>
#include <dbapserv.h>
#include <adslib.h>
#include <adui.h>
#include <acui.h>

#include <AddLineDialog.h>

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

class LineConfigDialog : public CPropertyPage
{
	DECLARE_DYNAMIC(LineConfigDialog)

public:
	LineConfigDialog( CWnd* pParentWnd = NULL );

	virtual ~LineConfigDialog();

// Dialog Data
	enum { IDD = IDD_DIALOG_LINE_CONFIG };

	afx_msg void OnBnClickedButtonAdd();
	afx_msg void OnBnClickedButtonMod();
	afx_msg void OnBnClickedButtonDel();
	
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

	BOOL UpdateLine(const int index,
					const wstring& rID,
					const wstring& rName,
					const wstring& rKind,
					const wstring& rCategory,
					const wstring& rShape,
					const wstring& rSize,
					const wstring& rEffectSize,
					const wstring& rUnit,
					const wstring& rComment);

	BOOL GetSelectData(LineConfigData& configData );

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL OnInitDialog();

	BOOL InitLineHeader();
	BOOL InitLineData();

	virtual BOOL OnNotify(WPARAM wParam, LPARAM lParam, LRESULT* pResult);

	DECLARE_MESSAGE_MAP()

private:
	int FindLine( const wstring& rName );

public:

	CAcUiListCtrl m_lineConfig;
};

} // end of config

} // end of assistant

} // end of guch

} // end of com