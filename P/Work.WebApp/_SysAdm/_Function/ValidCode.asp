<%
Rem JACK FIXED 20110106

Rem 動網論壇的驗證碼程式
Rem 動網7.1驗證碼字母向量庫的擴展

Rem 這個程式真是不錯，原作者自己實現的向量圖像變化
Rem 使用時，請到49行更改 image_code 為實際的 session 名稱

Rem NetDust 改了幾個小地方
Rem 2006-3-9 修正了一個可能除0的錯誤
Rem 2006-7-31 字元個數隨機，寬度、高度、位置隨機，加入干擾線


Rem ---------------------

Option Explicit

Rem 設置部分

Const nSaturation = 100     	' 色彩飽和度(100)
Const nBlankNoisyDotOdds = 0.00 ' 空白處噪點率(0.01)
Const nColorNoisyDotOdds = 0.00  ' 有色處噪點率(0.0)
Const nNoisyLineCount = 0    	' 噪音線條數(1)
Const nCharMin = 6       		' 產生的最小字元個數(5)
Const nCharMax = 6       		' 產生的最大字元個數(5)
Const nSpaceX = 2        		' 左右兩邊的空白寬度(2)
Const nSpaceY = 2        		' 上下兩邊的空白寬度(2)
Const nImgWidth = 100      		' 數據區寬度(100)
Const nImgHeight = 20      		' 資料區高度(20)
Const nCharWidthRandom = 1   	' 字元寬度隨機量(16)
Const nCharHeightRandom = 1  	' 字元高度隨機量(16)
Const nPositionXRandom = 1   	' 橫向位置隨機量(10)
Const nPositionYRandom = 1   	' 縱向位置隨機量(10)
Const nAngleRandom = 1          ' 筆劃角度隨機量(2)
Const nLengthRandom = 1         ' 筆劃長度隨機量(百分比)(2)
Const nColorHue = -2     		' 顯示驗證碼的色調(-1表示隨機色調, -2表示灰度色調)(-2)
'Const cCharSet = "0123456789BCDHIJKLMNPRX"
'Const cCharSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
Const cCharSet = "0123456789"
                                ' 構成驗證碼的字元集
                                ' 如果擴充了下邊的字母向量庫，則可以相應擴充這個字元集

Rem ---------------------

Response.Expires = -9999   
Response.AddHeader "Pragma","no-cache"  
Response.AddHeader "cache-ctrol","no-cache"  
Response.ContentType = "Image/BMP"  
Randomize

Dim Buf(), DigtalStr
Dim Lines(), LineCount
Dim CursorX, CursorY, DirX, DirY, nCharCount, nPixelWidth, nPixelHeight, PicWidth, PicHeight
	nCharCount = nCharMin + CInt(Rnd * (nCharMax - nCharMin))
	PicWidth = nImgWidth + 2 * nSpaceX
	PicHeight = nImgHeight + 2 * nSpaceY
	
	Call CreatValidCode("img_code")
	
Sub CreatValidCode(pSN)
Dim i, j, CurColorHue

 	CDGen
 	Session(pSN) = DigtalStr '記錄入Session

	Dim FileSize, PicDataSize
 	PicDataSize = PicWidth * PicHeight * 3
 	FileSize = PicDataSize + 54

	' 輸出BMP檔資訊頭
	Response.BinaryWrite ChrB(66) & ChrB(77) & ChrB(FileSize Mod 256) & ChrB((FileSize \ 256) Mod 256) & ChrB((FileSize \ 256 \ 256) Mod 256) & ChrB(FileSize \ 256 \ 256 \ 256) & ChrB(0) & ChrB(0) & ChrB(0) & ChrB(0) & ChrB(54) & ChrB(0) & ChrB(0) & ChrB(0)

	' 輸出BMP點陣圖資訊頭
	Response.BinaryWrite ChrB(40) & ChrB(0) & ChrB(0) & ChrB(0) & ChrB(PicWidth Mod 256) & ChrB((PicWidth \ 256) Mod 256) & ChrB((PicWidth \ 256 \ 256) Mod 256) & ChrB(PicWidth \ 256 \ 256 \ 256) & ChrB(PicHeight Mod 256) & ChrB((PicHeight \ 256) Mod 256) & ChrB((PicHeight \ 256 \ 256) Mod 256) & ChrB(PicHeight \ 256 \ 256 \ 256) & ChrB(1) & ChrB(0) & ChrB(24) & ChrB(0) & ChrB(0) & ChrB(0) & ChrB(0) & ChrB(0) & ChrB(PicDataSize Mod 256) & ChrB((PicDataSize \ 256) Mod 256) & ChrB((PicDataSize \ 256 \ 256) Mod 256) & ChrB(PicDataSize \ 256 \ 256 \ 256) & ChrB(18) & ChrB(11) & ChrB(0) & ChrB(0) & ChrB(18) & ChrB(11) & ChrB(0) & ChrB(0) & ChrB(0) & ChrB(0) & ChrB(0) & ChrB(0) & ChrB(0) & ChrB(0) & ChrB(0) & ChrB(0)

	' 逐點輸出點陣圖陣列
	If nColorHue = -1 Then
		CurColorHue = Int(Rnd * 360)
	ElseIf nColorHue <> -2 Then
		CurColorHue = nColorHue
	End If
	For j = 0 To PicHeight - 1
		For i = 0 To PicWidth - 1
			If nColorHue = -2 Then
				Response.BinaryWrite HSBToRGB(0, 0, 100 - Buf(PicHeight - 1 - j, i))
			Else
				Response.BinaryWrite HSBToRGB(CurColorHue, Buf(PicHeight - 1 - j, i), 100)
			End If
		Next
	Next
End Sub	

Sub CDGen_Reset()
 	' 重定向量筆和環境變數
 	LineCount = 0
	CursorX = 0
	 CursorY = 0
 	' 初始的光筆方向是垂直向下
 	DirX = 0
 	DirY = 1
End Sub

Sub CDGen_Clear()
' 清空位圖陣列
Dim i, j
ReDim Buf(PicHeight - 1, PicWidth - 1)

 	For j = 0 To PicHeight - 1
  		For i = 0 To PicWidth - 1
   			Buf(j, i) = 0
  		Next
 	Next
End Sub

Sub CDGen_PSet(X, Y)
 	' 在點陣圖陣列上畫點
 	If X >= 0 And X < PicWidth And Y >= 0 And Y < PicHeight Then Buf(Y, X) = 1
End Sub

Sub CDGen_Line(X1, Y1, X2, Y2)
' 在點陣圖陣列上畫線
Dim DX, DY, DeltaT, i

 	DX = X2 - X1
 	DY = Y2 - Y1
 	If Abs(DX) > Abs(DY) Then DeltaT = Abs(DX) Else DeltaT = Abs(DY)
 	If DeltaT = 0 Then
  		CDGen_PSet CInt(X1),CInt(Y1)
 	Else
  		For i = 0 To DeltaT
   			CDGen_PSet CInt(X1 + DX * i / DeltaT), CInt(Y1 + DY * i / DeltaT)
  		Next
 	End If
End Sub

Sub CDGen_FowardDraw(nLength)
	' 按當前光筆方向繪製指定長度並移動光筆，正數表示從左向右/從上向下繪製，負數表示從右向左/從下向上繪製
	nLength = nLength * (1 + (Rnd * 2 - 1) * nLengthRandom / 100)
	ReDim Preserve Lines(3, LineCount)
	Lines(0, LineCount) = CursorX
	Lines(1, LineCount) = CursorY
	CursorX = CursorX + DirX * nLength
	CursorY = CursorY + DirY * nLength
	Lines(2, LineCount) = CursorX
	Lines(3, LineCount) = CursorY
	LineCount = LineCount + 1
End Sub

Sub CDGen_SetDirection(nAngle)
' 按指定角度設定畫筆方向, 正數表示相對當前方向順時針改變方向，負數表示相對當前方向逆時針改變方向
Dim DX, DY
	
	nAngle = (nAngle + (Rnd * 2 - 1) * nAngleRandom) / 180 * 3.1415926
	DX = DirX
	DY = DirY
	DirX = DX * Cos(nAngle) - DY * Sin(nAngle)
	DirY = DX * Sin(nAngle) + DY * Cos(nAngle)
End Sub

Sub CDGen_MoveToMiddle(nActionIndex, nPercent)
' 將畫筆游標移動到指定動作的中間點上，nPercent為中間位置的百分比
Dim DeltaX, DeltaY
	
	DeltaX = Lines(2, nActionIndex) - Lines(0, nActionIndex)
	DeltaY = Lines(3, nActionIndex) - Lines(1, nActionIndex)
	CursorX = Lines(0, nActionIndex) + DeltaX * nPercent / 100
	CursorY = Lines(1, nActionIndex) + DeltaY * Abs(DeltaY) * nPercent / 100
End Sub

Sub CDGen_MoveCursor(nActionIndex)
	' 將畫筆游標移動到指定動作的起始點上
	CursorX = Lines(0, nActionIndex)
	CursorY = Lines(1, nActionIndex)
End Sub

Sub CDGen_Close(nActionIndex)
' 將當前光筆位置與指定動作的起始點閉合並移動光筆
ReDim Preserve Lines(3, LineCount)
	Lines(0, LineCount) = CursorX
	Lines(1, LineCount) = CursorY
	CursorX = Lines(0, nActionIndex)
	CursorY = Lines(1, nActionIndex)
	Lines(2, LineCount) = CursorX
	Lines(3, LineCount) = CursorY
	LineCount = LineCount + 1
End Sub

Sub CDGen_CloseToMiddle(nActionIndex, nPercent)
' 將當前光筆位置與指定動作的中間點閉合並移動光筆，nPercent為中間位置的百分比
Dim DeltaX, DeltaY
ReDim Preserve Lines(3, LineCount)
	Lines(0, LineCount) = CursorX
	Lines(1, LineCount) = CursorY
	DeltaX = Lines(2, nActionIndex) - Lines(0, nActionIndex)
	DeltaY = Lines(3, nActionIndex) - Lines(1, nActionIndex)
	CursorX = Lines(0, nActionIndex) + Sgn(DeltaX) * Abs(DeltaX) * nPercent / 100
	CursorY = Lines(1, nActionIndex) + Sgn(DeltaY) * Abs(DeltaY) * nPercent / 100
	Lines(2, LineCount) = CursorX
	Lines(3, LineCount) = CursorY
	LineCount = LineCount + 1
End Sub

Sub CDGen_Flush(X0, Y0)
	' 按照當前的畫筆動作序列繪製點陣圖點陣
	Dim MaxX, MinX, MaxY, MinY
	Dim DeltaX, DeltaY, StepX, StepY, OffsetX, OffsetY
	Dim i

	MaxX = MinX = MaxY = MinY = 0
	For i = 0 To LineCount - 1
		If MaxX < Lines(0, i) Then MaxX = Lines(0, i)
		If MaxX < Lines(2, i) Then MaxX = Lines(2, i)
		If MinX > Lines(0, i) Then MinX = Lines(0, i)
		If MinX > Lines(2, i) Then MinX = Lines(2, i)
		If MaxY < Lines(1, i) Then MaxY = Lines(1, i)
		If MaxY < Lines(3, i) Then MaxY = Lines(3, i)
		If MinY > Lines(1, i) Then MinY = Lines(1, i)
		If MinY > Lines(3, i) Then MinY = Lines(3, i)
	Next
	DeltaX = MaxX - MinX
	DeltaY = MaxY - MinY
	If DeltaX = 0 Then DeltaX = 1
	If DeltaY = 0 Then DeltaY = 1
	MaxX = MinX
	MaxY = MinY
	If DeltaX > DeltaY Then
		StepX = (nPixelWidth - 2) / DeltaX
		StepY = (nPixelHeight - 2) / DeltaX
		OffsetX = 0
		OffsetY = (DeltaX - DeltaY) / 2
	Else
		StepX = (nPixelWidth - 2) / DeltaY
		StepY = (nPixelHeight - 2) / DeltaY
		OffsetX = (DeltaY - DeltaX) / 2
		OffsetY = 0
	End If
	For i = 0 To LineCount - 1
		Lines(0, i) = Round((Lines(0, i) - MaxX + OffsetX) * StepX, 0)
		Lines(1, i) = Round((Lines(1, i) - MaxY + OffsetY) * StepY, 0)
		Lines(2, i) = Round((Lines(2, i) - MinX + OffsetX) * StepX, 0)
		Lines(3, i) = Round((Lines(3, i) - MinY + OffsetY) * StepY, 0)
		CDGen_Line Lines(0, i) + X0 + 1, Lines(1, i) + Y0 + 1, Lines(2, i) + X0 + 1, Lines(3, i) + Y0 + 1
	Next
End Sub

Sub CDGen_SetDirectionFormZero(nAngle)
	'按指定角度設定畫筆方向，與CDGen_SetDirection的區別是以0度為基準
	nAngle = Sgn(nAngle) * (Abs(nAngle) - nAngleRandom + Rnd * nAngleRandom * 2) / 180 * 3.1415926
	DirX = - Sin(nAngle)
	DirY = Cos(nAngle)
End Sub

Sub CDGen_Char(cChar, X0, Y0)
	' 在指定座標處生成指定字元的點陣圖陣列
	CDGen_Reset
 	Select Case cChar
 		Case "0"
			CDGen_SetDirection -60                            ' 逆時針60度(相對於垂直線)
			CDGen_FowardDraw -0.7                             ' 反方向繪製0.7個單位
			CDGen_SetDirection -60                            ' 逆時針60度
			CDGen_FowardDraw -0.7                             ' 反方向繪製0.7個單位
			CDGen_SetDirection 120                            ' 順時針120度
			CDGen_FowardDraw 1.5                              ' 繪製1.5個單位
			CDGen_SetDirection -60                            ' 逆時針60度
			CDGen_FowardDraw 0.7                              ' 繪製0.7個單位
			CDGen_SetDirection -60                            ' 順時針120度
			CDGen_FowardDraw 0.7                              ' 繪製0.7個單位
			CDGen_Close 0                                     ' 封閉當前筆與第0筆(0開始)
 		Case "1"
			CDGen_SetDirection -90                            ' 逆時針90度(相對於垂直線)
			CDGen_FowardDraw 0.5                              ' 繪製0.5個單位
			CDGen_MoveToMiddle 0, 50                          ' 移動畫筆的位置到第0筆(0開始)的50%處
			CDGen_SetDirection 90                             ' 逆時針90度
			CDGen_FowardDraw -1.4                             ' 反方向繪製1.4個單位
			CDGen_SetDirection 30                             ' 逆時針30度
			CDGen_FowardDraw 0.4                              ' 繪製0.4個單位
		Case "2"
			CDGen_SetDirection 45                             ' 順時針45度(相對於垂直線)
			CDGen_FowardDraw -0.7                             ' 反方向繪製0.7個單位
			CDGen_SetDirection -120                           ' 逆時針120度
			CDGen_FowardDraw 0.4                              ' 繪製0.4個單位
			CDGen_SetDirection 60                             ' 順時針60度
			CDGen_FowardDraw 0.6                              ' 繪製0.6個單位
			CDGen_SetDirection 60                             ' 順時針60度
			CDGen_FowardDraw 1.6                              ' 繪製1.6個單位
			CDGen_SetDirection -135                           ' 逆時針135度
			CDGen_FowardDraw 1.0                              ' 繪製1.0個單位
		Case "3"
			CDGen_SetDirection -90                            ' 逆時針90度(相對於垂直線)
			CDGen_FowardDraw 0.8                              ' 繪製0.8個單位
			CDGen_SetDirection 135                            ' 順時針135度
			CDGen_FowardDraw 0.8                              ' 繪製0.8個單位
			CDGen_SetDirection -120                           ' 逆時針120度
			CDGen_FowardDraw 0.6                              ' 繪製0.6個單位
			CDGen_SetDirection 80                             ' 順時針80度
			CDGen_FowardDraw 0.5                              ' 繪製0.5個單位
			CDGen_SetDirection 60                             ' 順時針60度
			CDGen_FowardDraw 0.5                              ' 繪製0.5個單位
			CDGen_SetDirection 60                             ' 順時針60度
			CDGen_FowardDraw 0.5                              ' 繪製0.5個單位
		Case "4"
			CDGen_SetDirection 20                             ' 順時針20度(相對於垂直線)
			CDGen_FowardDraw 0.8                              ' 繪製0.8個單位
			CDGen_SetDirection -110                           ' 逆時針110度
			CDGen_FowardDraw 1.2                              ' 繪製1.2個單位
			CDGen_MoveToMiddle 1, 60                          ' 移動畫筆的位置到第1筆(0開始)的60%處
			CDGen_SetDirection 90                             ' 順時針90度
			CDGen_FowardDraw 0.7                              ' 繪製0.7個單位
			CDGen_MoveCursor 2                                ' 移動畫筆到第2筆(0開始)的開始處
			CDGen_FowardDraw -1.5                             ' 反方向繪製1.5個單位
		Case "5"
			CDGen_SetDirection 90                             ' 順時針90度(相對於垂直線)
			CDGen_FowardDraw 1.0                              ' 繪製1.0個單位
			CDGen_SetDirection -90                            ' 逆時針90度
			CDGen_FowardDraw 0.8                              ' 繪製0.8個單位
			CDGen_SetDirection -90                            ' 逆時針90度
			CDGen_FowardDraw 0.8                              ' 繪製0.8個單位
			CDGen_SetDirection 30                             ' 順時針30度
			CDGen_FowardDraw 0.4                              ' 繪製0.4個單位
			CDGen_SetDirection 60                             ' 順時針60度
			CDGen_FowardDraw 0.4                              ' 繪製0.4個單位
			CDGen_SetDirection 30                             ' 順時針30度
			CDGen_FowardDraw 0.5                              ' 繪製0.5個單位
			CDGen_SetDirection 60                             ' 順時針60度
			CDGen_FowardDraw 0.8                              ' 繪製0.8個單位
		Case "6"
			CDGen_SetDirection -60                            ' 逆時針60度(相對於垂直線)
			CDGen_FowardDraw -0.7                             ' 反方向繪製0.7個單位
			CDGen_SetDirection -60                            ' 逆時針60度
			CDGen_FowardDraw -0.7                             ' 反方向繪製0.7個單位
			CDGen_SetDirection 120                            ' 順時針120度
			CDGen_FowardDraw 1.5                              ' 繪製1.5個單位
			CDGen_SetDirection 120                            ' 順時針120度
			CDGen_FowardDraw -0.7                             ' 反方向繪製0.7個單位
			CDGen_SetDirection 120                            ' 順時針120度
			CDGen_FowardDraw 0.7                              ' 繪製0.7個單位
			CDGen_SetDirection 120                            ' 順時針120度
			CDGen_FowardDraw -0.7                             ' 反方向繪製0.7個單位
			CDGen_SetDirection 120                            ' 順時針120度
			CDGen_FowardDraw 0.5                              ' 繪製0.5個單位
			CDGen_CloseToMiddle 2, 50                         ' 將當前畫筆位置與第2筆(0開始)的50%處封閉
		Case "7"
			CDGen_SetDirection 180                            ' 順時針180度(相對於垂直線)
			CDGen_FowardDraw 0.3                              ' 繪製0.3個單位
			CDGen_SetDirection 90                             ' 順時針90度
			CDGen_FowardDraw 0.9                              ' 繪製0.9個單位
			CDGen_SetDirection 120                            ' 順時針120度
			CDGen_FowardDraw 1.3                              ' 繪製1.3個單位
		Case "8"
			CDGen_SetDirection -60                            ' 逆時針60度(相對於垂直線)
			CDGen_FowardDraw -0.8                             ' 反方向繪製0.8個單位
			CDGen_SetDirection -60                            ' 逆時針60度
			CDGen_FowardDraw -0.8                             ' 反方向繪製0.8個單位
			CDGen_SetDirection 120                            ' 順時針120度
			CDGen_FowardDraw 0.8                              ' 繪製0.8個單位
			CDGen_SetDirection 110                            ' 順時針110度
			CDGen_FowardDraw -1.5                             ' 反方向繪製1.5個單位
			CDGen_SetDirection -110                           ' 逆時針110度
			CDGen_FowardDraw 0.9                              ' 繪製0.9個單位
			CDGen_SetDirection 60                             ' 順時針60度
			CDGen_FowardDraw 0.8                              ' 繪製0.8個單位
			CDGen_SetDirection 60                             ' 順時針60度
			CDGen_FowardDraw 0.8                              ' 繪製0.8個單位
			CDGen_SetDirection 60                             ' 順時針60度
			CDGen_FowardDraw 0.9                              ' 繪製0.9個單位
			CDGen_SetDirection 70                             ' 順時針70度
			CDGen_FowardDraw 1.5                             ' 繪製1.5個單位
			CDGen_Close 0                                     ' 封閉當前筆與第0筆(0開始)
		Case "9"
			CDGen_SetDirection 120                            ' 逆時針60度(相對於垂直線)
			CDGen_FowardDraw -0.7                             ' 反方向繪製0.7個單位
			CDGen_SetDirection -60                            ' 逆時針60度
			CDGen_FowardDraw -0.7                             ' 反方向繪製0.7個單位
			CDGen_SetDirection -60                            ' 順時針120度
			CDGen_FowardDraw -1.5                              ' 繪製1.5個單位
			CDGen_SetDirection -60                            ' 順時針120度
			CDGen_FowardDraw -0.7                             ' 反方向繪製0.7個單位
			CDGen_SetDirection -60                            ' 順時針120度
			CDGen_FowardDraw -0.7                              ' 繪製0.7個單位
			CDGen_SetDirection 120                            ' 順時針120度
			CDGen_FowardDraw 0.7                             ' 反方向繪製0.7個單位
			CDGen_SetDirection -60                            ' 順時針120度
			CDGen_FowardDraw 0.5                              ' 繪製0.5個單位
			CDGen_CloseToMiddle 2, 50                         ' 將當前畫筆位置與第2筆(0開始)的50%處封閉
		' 以下為字母的向量動作，有興趣的可以繼續
		Case "A"
			CDGen_SetDirection -(Rnd * 20 + 150)              ' 逆時針150-170度(相對於垂直線)
			CDGen_FowardDraw Rnd * 0.2 + 1.1                  ' 繪製1.1-1.3個單位
			CDGen_SetDirection Rnd * 20 + 140                 ' 順時針140-160度
			CDGen_FowardDraw Rnd * 0.2 + 1.1                  ' 繪製1.1-1.3個單位
			CDGen_MoveToMiddle 0, 30                          ' 移動畫筆的位置到第1筆(0開始)的30%處
			CDGen_CloseToMiddle 1, 70                         ' 將當前畫筆位置與第1筆(0開始)的70%處封閉
		Case "B"
			CDGen_SetDirection -(Rnd * 20 + 50)               ' 逆時針50-70度(相對於垂直線)
			CDGen_FowardDraw Rnd * 0.4 + 0.8                  ' 繪製0.8-1.2個單位
			CDGen_SetDirection Rnd * 20 + 110                 ' 順時針110-130度
			CDGen_FowardDraw Rnd * 0.2 + 0.6                  ' 繪製0.6-0.8個單位
			CDGen_SetDirection -(Rnd * 20 + 110)              ' 逆時針110-130度
			CDGen_FowardDraw Rnd * 0.2 + 0.6                  ' 繪製0.6-0.8個單位
			CDGen_SetDirection Rnd * 20 + 110                 ' 順時針110-130度
			CDGen_FowardDraw Rnd * 0.4 + 0.8                  ' 繪製0.8-1.2個單位
			CDGen_Close 0                                     ' 封閉當前筆與第1筆(0開始)
		Case "C"
			CDGen_SetDirection -60                            ' 逆時針60度(相對於垂直線)
			CDGen_FowardDraw -0.7                             ' 反方向繪製0.7個單位
			CDGen_SetDirection -60                            ' 逆時針60度
			CDGen_FowardDraw -0.7                             ' 反方向繪製0.7個單位
			CDGen_SetDirection 120                            ' 順時針120度
			CDGen_FowardDraw 1.5                              ' 繪製1.5個單位
			CDGen_SetDirection 120                            ' 順時針120度
			CDGen_FowardDraw -0.7                             ' 反方向繪製0.7個單位
			CDGen_SetDirection 120                            ' 順時針120度
			CDGen_FowardDraw 0.7                              ' 繪製0.7個單位
		Case "D"
			CDGen_SetDirection -(Rnd * 40 - 20)
			CDGen_FowardDraw 1
			CDGen_SetDirection -120
			CDGen_FowardDraw 0.4
			CDGen_SetDirection -45
			CDGen_FowardDraw 0.5
			CDGen_Close 0
		Case "E"
			CDGen_SetDirection -(Rnd * 20 - 10)
			CDGen_FowardDraw 1
			CDGen_MoveToMiddle 0, 0
			CDGen_SetDirectionFormZero -(110 - Rnd * 40)
			CDGen_FowardDraw 0.7
			CDGen_MoveToMiddle 0, 50
			CDGen_SetDirectionFormZero -(110 - Rnd * 40)
			CDGen_FowardDraw 0.5
			CDGen_MoveToMiddle 0, 100
			CDGen_SetDirectionFormZero -(110 - Rnd * 40)
			CDGen_FowardDraw 0.9
		Case "F"
			CDGen_SetDirection -(Rnd * 20 - 10)
			CDGen_FowardDraw 1
			CDGen_MoveToMiddle 0, 0
			CDGen_SetDirectionFormZero -(110 - Rnd * 40)
			CDGen_FowardDraw 0.7
			CDGen_MoveToMiddle 0, 50
			CDGen_SetDirectionFormZero -(110 - Rnd * 40)
			CDGen_FowardDraw 0.5
		Case "G"
			CDGen_SetDirection -60
			CDGen_FowardDraw -0.7
			CDGen_SetDirection -60
			CDGen_FowardDraw -0.7
			CDGen_SetDirection 120
			CDGen_FowardDraw 1.5
			CDGen_SetDirection 120
			CDGen_FowardDraw -0.7
			CDGen_SetDirection 120
			CDGen_FowardDraw 0.7
			CDGen_SetDirection -60
			CDGen_FowardDraw 0.5
			CDGen_SetDirection 90
			CDGen_FowardDraw 0.4
			CDGen_MoveToMiddle 6, 0
			CDGen_SetDirection 180
			CDGen_FowardDraw 0.4
		Case "H"
			CDGen_SetDirection -(Rnd * 20 - 10)
			CDGen_FowardDraw 1
			CDGen_MoveToMiddle 0, 50
			CDGen_SetDirection -90
			CDGen_FowardDraw 1
			CDGen_MoveToMiddle 1, 100
			CDGen_SetDirection -90
			CDGen_FowardDraw 0.5
			CDGen_MoveToMiddle 1, 100
			CDGen_SetDirection 180
			CDGen_FowardDraw 0.5
		Case "I"
			CDGen_SetDirection -(Rnd * 20 - 10)
			CDGen_FowardDraw 1
			CDGen_MoveToMiddle 0, 0
			CDGen_SetDirection -90
			CDGen_FowardDraw 0.2
			CDGen_MoveToMiddle 0, 0
			CDGen_SetDirection 180
			CDGen_FowardDraw 0.2
			CDGen_MoveToMiddle 0, 100
			CDGen_FowardDraw 0.2
			CDGen_MoveToMiddle 0, 100
			CDGen_SetDirection 180
			CDGen_FowardDraw 0.2
		Case "J"
			CDGen_SetDirection -90
			CDGen_FowardDraw 0.4
			CDGen_MoveToMiddle 0, 50
			CDGen_SetDirection 90
			CDGen_FowardDraw 0.6
			CDGen_SetDirection 60
			CDGen_FowardDraw 0.5
			CDGen_SetDirection 120
			CDGen_FowardDraw 0.5
		Case "K"
			CDGen_SetDirection -(Rnd * 20 - 10)
			CDGen_FowardDraw 1
			CDGen_MoveToMiddle 0, 50
			CDGen_SetDirection -60
			CDGen_FowardDraw 0.6
			CDGen_MoveToMiddle 0, 50
			CDGen_SetDirection -60
			CDGen_FowardDraw 0.6
		Case "L"
			CDGen_SetDirection -90
			CDGen_FowardDraw 0.2
			CDGen_MoveToMiddle 0, 50
			CDGen_SetDirectionFormZero -(Rnd * 20 - 10)
			CDGen_FowardDraw 1
			CDGen_SetDirection -(110 - Rnd * 40)
			CDGen_FowardDraw 0.8
			CDGen_SetDirectionFormZero 0
			CDGen_FowardDraw -0.3
		Case "M"
			CDGen_SetDirection 0
			CDGen_FowardDraw -1
			CDGen_SetDirection -30
			CDGen_FowardDraw 0.5
			CDGen_SetDirection 60
			CDGen_FowardDraw -0.5
			CDGen_SetDirection -30
			CDGen_FowardDraw 1
		Case "N"
			CDGen_SetDirection 0
			CDGen_FowardDraw -1
			CDGen_SetDirection -45
			CDGen_FowardDraw 1.4
			CDGen_SetDirection 45
			CDGen_FowardDraw -1
		Case "O"
			CDGen_SetDirection -60
			CDGen_FowardDraw -0.7
			CDGen_SetDirection -60
			CDGen_FowardDraw -0.7
			CDGen_SetDirection 120
			CDGen_FowardDraw 1.5
			CDGen_SetDirection -60
			CDGen_FowardDraw 0.7
			CDGen_SetDirection -60
			CDGen_FowardDraw 0.7
			CDGen_Close 0
		Case "P"
			CDGen_SetDirection 0
			CDGen_FowardDraw -1
			CDGen_SetDirection -60
			CDGen_FowardDraw 0.5
			CDGen_SetDirection 60
			CDGen_FowardDraw 0.5
			CDGen_CloseToMiddle 0, 50
		Case "Q"
			CDGen_SetDirection -60
			CDGen_FowardDraw -0.7
			CDGen_SetDirection -60
			CDGen_FowardDraw -0.7
			CDGen_SetDirection 120
			CDGen_FowardDraw 1.5
			CDGen_SetDirection -60
			CDGen_FowardDraw 0.7
			CDGen_SetDirection -60
			CDGen_FowardDraw 0.7
			CDGen_Close 0
			CDGen_MoveToMiddle 4, 100
			CDGen_SetDirectionFormZero -45
			CDGen_FowardDraw 0.7
			CDGen_MoveToMiddle 4, 100
			CDGen_SetDirection 180
			CDGen_FowardDraw 0.7
		Case "R"
			CDGen_SetDirection 0
			CDGen_FowardDraw -1
			CDGen_SetDirection -80
			CDGen_FowardDraw 0.5
			CDGen_SetDirection 60
			CDGen_FowardDraw 0.5
			CDGen_CloseToMiddle 0, 50
			CDGen_SetDirectionFormZero -45
			CDGen_FowardDraw 0.7
		Case "S"
			CDGen_SetDirection -45
			CDGen_FowardDraw -0.5
			CDGen_SetDirection -90
			CDGen_FowardDraw -0.5
			CDGen_SetDirection 90
			CDGen_FowardDraw 1
			CDGen_SetDirection 90
			CDGen_FowardDraw 0.5
			CDGen_SetDirection 90
			CDGen_FowardDraw 0.5
		Case "T"
			CDGen_SetDirection -90
			CDGen_FowardDraw 0.8
			CDGen_MoveToMiddle 0, 50
			CDGen_SetDirection 90
			CDGen_FowardDraw 1
			CDGen_MoveToMiddle 0, 0
			CDGen_SetDirection 30
			CDGen_FowardDraw 0.5
			CDGen_MoveToMiddle 0, 100
			CDGen_SetDirection -60
			CDGen_FowardDraw 0.5
		Case "U"
			CDGen_FowardDraw 1
			CDGen_SetDirection -60
			CDGen_FowardDraw 0.6
			CDGen_SetDirection -60
			CDGen_FowardDraw 0.6
			CDGen_SetDirection -60
			CDGen_FowardDraw 1
		Case "V"
			CDGen_SetDirection -30
			CDGen_FowardDraw 1.5
			CDGen_SetDirection 60
			CDGen_FowardDraw -1.5
		Case "W"
			CDGen_SetDirection -30
			CDGen_FowardDraw 1.5
			CDGen_SetDirection 60
			CDGen_FowardDraw -1
			CDGen_SetDirection -60
			CDGen_FowardDraw 1
			CDGen_SetDirection 60
			CDGen_FowardDraw -1.5
		Case "X"
			CDGen_SetDirection -45
			CDGen_FowardDraw 1.4
			CDGen_MoveToMiddle 0, 50
			CDGen_SetDirection 90
			CDGen_FowardDraw 0.7
			CDGen_MoveToMiddle 0, 50
			CDGen_FowardDraw -0.7
		Case "Y"
			CDGen_SetDirection -30
			CDGen_FowardDraw 0.5
			CDGen_SetDirection 60
			CDGen_FowardDraw -0.5
			CDGen_MoveToMiddle 0, 100
			CDGen_SetDirection -30
			CDGen_FowardDraw 0.5
		Case "Z"
			CDGen_SetDirection -90
			CDGen_FowardDraw 1
			CDGen_SetDirection -45
			CDGen_FowardDraw -1.4
			CDGen_SetDirection 45
			CDGen_FowardDraw 1
	End Select
	CDGen_Flush X0, Y0
End Sub

Sub CDGen_Blur()
' 對產生的點陣圖進行柔化處理
Dim i, j

 	For j = 1 To PicHeight - 2
  		For i = 1 To PicWidth - 2
   			If Buf(j, i) = 0 Then
    			If ((Buf(j, i - 1) Or Buf(j + 1, i)) And 1) <> 0 Then
     				' 如果當前點是空白點，且上下左右四個點中有一個點是有色點，則該點做柔化處理
     				Buf(j, i) = 2
    			End If
   			End If
  		Next
 	Next
End Sub

Sub CDGen_NoisyLine()
Dim i
 	For i=1 To nNoisyLineCount
  		CDGen_Line Rnd * PicWidth, Rnd * PicHeight, Rnd * PicWidth, Rnd * PicHeight
 	Next
End Sub

Sub CDGen_NoisyDot()
' 對產生的點陣圖進行噪點處理
Dim i, j, NoisyDot, CurDot

 	For j = 0 To PicHeight - 1
  		For i = 0 To PicWidth - 1
   			If Buf(j, i) <> 0 Then
    			If Rnd < nColorNoisyDotOdds Then
     				Buf(j, i) = 8
    			Else
     				Buf(j, i) = nSaturation
    			End If
   			Else
    			If Rnd < nBlankNoisyDotOdds Then
     				Buf(j, i) = nSaturation
    			Else
     				Buf(j, i) =8
    			End If
   			End If
  		Next
 	Next
End Sub

Sub CDGen()
' 生成點陣圖陣列
Dim i, Ch, w, x, y

 	DigtalStr = ""
 	CDGen_Clear
 	w = nImgWidth / nCharCount

 	For i = 0 To nCharCount - 1
		nPixelWidth = w * (1 + (Rnd * 2 - 1) * nCharWidthRandom / 100)
		nPixelHeight = nImgHeight * (1 - Rnd * nCharHeightRandom / 100)
		x = nSpaceX + w * (i + (Rnd * 2 - 1) * nPositionXRandom / 100)
		y = nSpaceY + nImgHeight * (Rnd * 2 - 1) * nPositionYRandom / 100

		Ch = Mid(cCharSet, Int(Rnd * Len(cCharSet)) + 1, 1)
		DigtalStr = DigtalStr + Ch
		
		CDGen_Char Ch, x, y
 	Next
	CDGen_Blur
	CDGen_NoisyLine
	CDGen_NoisyDot
End Sub

Function HSBToRGB(vH, vS, vB)
' 將顏色值由HSB轉換為RGB
Dim aRGB(3), RGB1st, RGB2nd, RGB3rd
Dim nH, nS, nB
Dim lH, nF, nP, nQ, nT

 	vH = (vH Mod 360)
 	If vS > 100 Then
  		vS = 100
 	ElseIf vS < 0 Then
  		vS = 0
 	End If
 	If vB > 100 Then
  		vB = 100
 	ElseIf vB < 0 Then
  		vB = 0
 	End If
 	If vS > 0 Then
		nH = vH / 60
		nS = vS / 100
		nB = vB / 100
		lH = Int(nH)
		nF = nH - lH
		nP = nB * (1 - nS)
		nQ = nB * (1 - nS * nF)
		nT = nB * (1 - nS * (1 - nF))
  		Select Case lH
			Case 0
				aRGB(0) = nB * 255
				aRGB(1) = nT * 255
				aRGB(2) = nP * 255
			Case 1
				aRGB(0) = nQ * 255
				aRGB(1) = nB * 255
				aRGB(2) = nP * 255
			Case 2
				aRGB(0) = nP * 255
				aRGB(1) = nB * 255
				aRGB(2) = nT * 255
			Case 3
				aRGB(0) = nP * 255
				aRGB(1) = nQ * 255
				aRGB(2) = nB * 255
			Case 4
				aRGB(0) = nT * 255
				aRGB(1) = nP * 255
				aRGB(2) = nB * 255
			Case 5
				aRGB(0) = nB * 255
				aRGB(1) = nP * 255
				aRGB(2) = nQ * 255
		End Select
 	Else
		aRGB(0) = (vB * 255) / 100
		aRGB(1) = aRGB(0)
		aRGB(2) = aRGB(0)
 	End If
 	HSBToRGB = ChrB(Round(aRGB(2), 0)) & ChrB(Round(aRGB(1), 0)) & ChrB(Round(aRGB(0), 0))
End Function


%>

