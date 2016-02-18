function ins_chk(f) {
var Obj = document.v;
var strMsg,intFlag
   strMsg = ""
   intFlag = 0

if (Obj.flg.value == "ok") {
		if (isNaN(f.p14.value)) {
			alert ("金額請輸入數字!");
			Obj.p14.focus();
			return false;
		}

		if (!chkField("p1")) {
			alert ("請輸入姓名!");
			Obj.p1.focus();
			return false;
		}
		
		if (!chkField("sex")) {
			alert ("請選擇性別!");
			return false;
		}
		
		if (!chkField("p2")) {
			alert ("請選擇生日!");
			return false;
		}
		
		if (!chkField("p3")) {
			alert ("請輸入E-mail!");
			Obj.p3.focus();
			return false;
		}
		if (!chkField("p4")) {
			alert ("請輸入電話區碼!");
			Obj.p4.focus();
			return false;
		}
		if (isNaN(f.p4.value)) {
			alert ("電話區碼請輸入數字!");
			Obj.p4.focus();
			return false;
		}
		if (!chkField("p5")) {
			alert ("請輸入電話尾碼!");
			Obj.p5.focus();
			return false;
		}
		if (isNaN(f.p5.value)) {
			alert ("電話尾碼請輸入數字!");
			Obj.p5.focus();
			return false;
		}
		if (!chkField("p6")) {
			alert ("請輸入行動電話!");
			Obj.p5.focus();
			return false;
		}
		if (isNaN(f.p6.value)) {
			alert ("行動電話請輸入數字!");
			Obj.p6.focus();
			return false;
		}
		if (!chkField("p7")) {
			alert ("請輸入住址!");
			Obj.p7.focus();
			return false;
		}
		if (isNaN(f.p15.value)) {
			alert ("郵遞區號請輸入數字!");
			Obj.p15.focus();
			return false;
		}
		if (!chkField("p15")) {
			alert ("請輸入郵遞區號!");
			Obj.p15.focus();
			return false;
		}
		
	} 
	return true;
}