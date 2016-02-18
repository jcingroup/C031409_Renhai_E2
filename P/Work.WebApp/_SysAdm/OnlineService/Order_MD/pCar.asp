<%
Sub ClearistOrderDetail()
	Session.Contents.Remove("SYSOrderList")
End Sub

Function FamilyOptions(MasterID)
	Dim sql,rs
	
	sql = "Select 序號,姓名 From 會員資料表 Where 戶長SN=" & MasterID
	Set rs = ExecSQL_RTN_RST(sql,3,1,2)
	FamilyOptions = "<option value=""""></option>" & RsToOption(rs,0,1,"","","")
End Function

Sub SerialToCarSession(OrderSerial)

	Dim Order_NO
	Dim dicCP,dicCP_DataItemCollect,dicCP_DataItems
	Dim ItemsIndex
	Dim rs_Master,rs,sql
	
	sql="Select * From 訂單主檔 WHERE 訂單序號=" & OrderSerial
	Set rs_Master = ExecSQL_RTN_RST(sql,3,1,2)

	Order_NO = rs_Master.Fields("訂單編號").Value
	sql="Select * From 訂單明細檔 WHERE 訂單編號 = '" & Order_NO & "'"
	Set rs = ExecSQL_RTN_RST(sql,3,1,2)
	
	ClearistOrderDetail

	Set dicCP			= Server.CreateObject("Scripting.Dictionary")
	Set dicCP_DataItemCollect	= Server.CreateObject("Scripting.Dictionary")

	DicKeyValue dicCP,"戶長SN",rs_Master.Fields("戶長SN").Value
	DicKeyValue dicCP,"申請人姓名",rs_Master.Fields("申請人姓名").Value
	DicKeyValue dicCP,"會員編號",rs_Master.Fields("會員編號").Value
	
	Do Until rs.Eof
		
		Set dicCP_DataItems = Server.CreateObject("Scripting.Dictionary")

		DicKeyValue dicCP_DataItems,"訂單序號",rs.Fields("訂單序號").Value
		DicKeyValue dicCP_DataItems,"數量",rs.Fields("數量").Value
		DicKeyValue dicCP_DataItems,"戶長SN",rs_Master.Fields("戶長SN").Value
	
		DicKeyValue dicCP_DataItems,"價格",rs.Fields("價格").Value	
		DicKeyValue dicCP_DataItems,"產品編號",rs.Fields("產品編號").Value
		DicKeyValue dicCP_DataItems,"產品名稱",rs.Fields("產品名稱").Value
		
		DicKeyValue dicCP_DataItems,"會員編號",rs.Fields("會員編號").Value
		DicKeyValue dicCP_DataItems,"申請人姓名",rs.Fields("申請人姓名").Value
		DicKeyValue dicCP_DataItems,"申請人地址",rs.Fields("申請人地址").Value
		DicKeyValue dicCP_DataItems,"申請人性別",rs.Fields("申請人性別").Value
		DicKeyValue dicCP_DataItems,"申請人時辰",rs.Fields("申請人時辰").Value
		DicKeyValue dicCP_DataItems,"申請人生日",rs.Fields("申請人生日").Value
		DicKeyValue dicCP_DataItems,"申請人年齡",rs.Fields("申請人年齡").Value

		DicKeyValue dicCP_DataItems,"購買時間",rs.Fields("購買時間").Value
		DicKeyValue dicCP_DataItems,"付款時間",rs.Fields("付款時間").Value
		DicKeyValue dicCP_DataItems,"祈福事項",rs.Fields("祈福事項").Value
		DicKeyValue dicCP_DataItems,"郵遞區號",rs.Fields("郵遞區號").Value
		DicKeyValue dicCP_DataItems,"點燈位置",rs.Fields("點燈位置").Value
	
		DicKeyValue dicCP_DataItems,"白米",rs.Fields("白米").Value
		DicKeyValue dicCP_DataItems,"金牌",rs.Fields("金牌").Value
		
		DicKeyValue dicCP_DataItems,"文疏",rs.Fields("文疏梯次").Value


		ItemsIndex = MasterID & "." & rs.Fields("申請人姓名").Value & "." & rs.Fields("產品編號").Value

		DicKeyValue dicCP_DataItemCollect,ItemsIndex,dicCP_DataItems

		Set dicCP_DataItems = Nothing
		rs.MoveNext
	Loop

	DicKeyValue dicCP,"Data",dicCP_DataItemCollect
	Set Session("SYSOrderList") = dicCP
End Sub

Function ListOrderDetail()
	
	Dim rturnHTML
	Dim dicCP,dicCP_DataItemCollect,dicCP_DataItems
	Dim GetKeys,GetItems,GetKey
	Dim rowTpl,getRowTpl '組購物清單明細
	Dim i
	Dim total,total_race,total_gold
	
	rturnHTML = ""
	total = 0
	total_race = 0
	total_gold =0
	
	If IsObject(Session("SYSOrderList")) Then

		Set dicCP			= Session("SYSOrderList")
		Set dicCP_DataItemCollect	= dicCP.Item("Data")
		ErrCheck "Get DataCollect Err"
		rowTpl = "<tr class=""TableBodyTR"">" _
		& "<td class=""TableBodyTD""><prod ProdType=""{6}"" />{0}</td>" _
		& "<td class=""TableBodyTD"">{1}</td>" _
		& "<td class=""TableBodyTdNum"">{3}</td>" _
		& "<td class=""TableBodyTdNum"">{4}</td>" _
		& "<td class=""TableBodyTdNum"">{5}</td>" _
		& "<td class=""TableBodyTD""><a href=""javascript:ModifyDetail('{2}')"">檢視</a></td>"					
		'& "<td class=""TableBodyTD""><a href=""javascript:DeleteDetail('{2}')"">刪除</a></td>" _
		'& "</tr>"
		
		'管理者才有刪除權
		'If Session("UserKind")=99 Then
			rowTpl = rowTpl & "<td class=""TableBodyTD""><a href=""javascript:DeleteDetail('{2}')"">刪除</a></td>"
		'End If
		
		rowTpl = rowTpl & "</tr>"

		'& "<td class=""TableBodyTD""><a href=""javascript:ModifyDetail('{2}')"">修改</a></td>" _
		If dicCP_DataItemCollect.Count > 0 Then
			
			GetKeys = dicCP_DataItemCollect.Keys
			GetItems = dicCP_DataItemCollect.Items

			For i = 0 To dicCP_DataItemCollect.Count - 1
  				GetKey 			= GetKeys(i)
  				Set dicCP_DataItems 	= GetItems(i)
 				ErrCheck "Get DataItems Err"
 				
   				getRowTpl = rowTpl
   				getRowTpl = Replace(getRowTpl,"{0}",dicCP_DataItems.Item("申請人姓名"))
   				getRowTpl = Replace(getRowTpl,"{1}",dicCP_DataItems.Item("產品名稱"))
    			getRowTpl = Replace(getRowTpl,"{2}",dicCP_DataItems.Item("戶長SN") & "." & dicCP_DataItems.Item("申請人姓名") & "." & dicCP_DataItems.Item("產品編號"))  
 				getRowTpl = Replace(getRowTpl,"{3}",dicCP_DataItems.Item("價格"))  
 				getRowTpl = Replace(getRowTpl,"{4}",dicCP_DataItems.Item("白米"))  
 				getRowTpl = Replace(getRowTpl,"{5}",dicCP_DataItems.Item("金牌"))
 				getRowTpl = Replace(getRowTpl,"{6}",dicCP_DataItems.Item("產品編號")) 
 				 				  																
   				ErrCheck "Get DataItems Err"		
 				rturnHTML = rturnHTML & getRowTpl
 				total = total + dicCP_DataItems.Item("價格")
 				total_race = total_race + dicCP_DataItems.Item("白米")
 				total_gold = total_gold + dicCP_DataItems.Item("金牌")
 				ErrCheck "Total ADD ERR"
			Next
		End If
		
	rturnHTML = rturnHTML & "<tr class=""TableBodyTR""><td class=""TableBodyTD"" colspan=""2"" align=""right"">總計：</td>" _
			& "<td class=""TableBodyTdNum"">" & total & "</td><td class=""TableBodyTdNum"">" & total_race & "</td><td class=""TableBodyTdNum"">" & total_gold & "</td><td colspan=""2"" class=""TableBodyTD"">&nbsp;</td></tr>"
	ErrCheck "Html Sting"
	End If

	ListOrderDetail = rturnHTML

End Function
%>