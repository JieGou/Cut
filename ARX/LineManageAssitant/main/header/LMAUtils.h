#pragma once
#include <afxwin.h> 

#include <string>

using namespace std;

//将string转换成wstring
wstring StringToWString(const string& str);

//将wstring转换成string     
string WstringToString(const wstring& wstr);

// Enables and disables an MFC control by name
void EnableControl( CWnd* pWnd, int iControl, bool enable );

void ShowControl( CWnd *pWnd, int iControl, bool show );

void MoveControl( CWnd *pWnd, int iControl, int top, int left, int sizex, int sizey );

void OverControl( CWnd *pWnd, int iControlSrc, int iControlDst );

void SetControlText( CWnd* pWnd, int iControl, LPCTSTR lpszString );
