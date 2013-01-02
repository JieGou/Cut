#pragma once

#include "afxcmn.h"
#include <resource.h>

#include <dbsymtb.h>
#include <dbapserv.h>
#include <adslib.h>
#include <adui.h>
#include <acui.h>

#include <string>

using namespace std;

namespace com
{

namespace guch
{

namespace assistent
{

namespace entry
{

//实体管理窗口
class EntryManageDialog : public CDialog
{
	DECLARE_DYNAMIC(EntryManageDialog)

public:
	EntryManageDialog(CWnd* pParent = NULL);   // standard constructor
	virtual ~EntryManageDialog();

// Dialog Data
	enum { IDD = IDD_DIALOG_ENTRY_MANAGE };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL OnInitDialog();

	BOOL InitLineList();
	BOOL InitLineDetailHeader();
	BOOL InitLineDetailData();

	DECLARE_MESSAGE_MAP()

private:

	CTreeCtrl m_LinesTree;
	CListCtrl m_LineDetailList;

	CEdit m_LineNO;
	CEdit m_LineName;
	CEdit m_LineKind;

	CButton m_FlatView;
	CButton m_LevelView;

	CButton m_LineAdd;
	CButton m_LineDelete;

	wstring m_fileName;
};

} // end of config

} // end of assistant

} // end of guch

} // end of com
