#pragma once

#include "afxcmn.h"
#include <resource.h>

#include <dbsymtb.h>
#include <dbapserv.h>
#include <adslib.h>
#include <adui.h>
#include <acui.h>

#include <string>
#include <LineEntryData.h>

using namespace std;

using namespace com::guch::assistant::data;

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

	BOOL InitEntryList();
	BOOL InitEntryDetailHeader();
	BOOL InitEntryDetailData();

	BOOL InsertLine( LineEntry* lineEntry, BOOL bInitialize = FALSE );
	HTREEITEM GetKindNode( const wstring& category, const wstring& kind);

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

	LineEntryFile* m_EntryFile;

	HTREEITEM m_lineRoot;
	HTREEITEM m_blockRoot;
};

} // end of config

} // end of assistant

} // end of guch

} // end of com
