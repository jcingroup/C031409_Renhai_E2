//加入我的最愛
function bookmark(myFav,myTitle) {
	if (navigator.appName!="Netscape"){
		window.external.AddFavorite(myFav, myTitle);
	} else {
		window.location = myFav;
	}
}
//設為首頁
function setHome(){
	home.style.behavior="url(#default#homepage)";
	home.setHomePage("http://www.kongpower.com");
}

//訂閱電子報
function showPaper(path){
var txt;
	txt=path + "paper/paper.asp";
	windowopen("Paper",txt,320,195,0);
}

//會員登入
function login(Obj){
	if (Obj.field1.value=="") {
		alert ("請輸入會員帳號!");
		Obj.field1.focus();
		return false;
	}
	if (Obj.field2.value=="") {
		alert ("請輸入會員密碼!");
		Obj.field2.focus();
		return false;
	}
	Obj.flg.value="login";
	return true;
}
//查詢密碼
function forget(){
	//windowopen("forget","../member/search.asp",320,200,0);
	location.href="../member/search.asp";
}

//檢查表單欄位是否空白
//chkField(輸入格名稱)
function chkField(txt){
	var i=0;
	var t="",kind="";
	for(i=0;i<document.w.elements.length;i++) {
  		if (document.w.elements[i].name == txt) {
  			switch(document.w.elements[i].type){
				case "text":			kind="a";break;
				case "password":		kind="a";break;
				case "textarea":		kind="a";break;
				case "hidden":			kind="a";break;
				case "radio":			kind="b";break;
				case "checkbox":		kind="b";break;
				case "select-one":		kind="c";break;
				case "select-multiple":	kind="c";break;
				case "file":			kind="a";break;
			}
  			switch(kind){
  				case "a":
  					t = document.w.elements[i].value;
  					break;
		   		case "b":
		   			if (document.w.elements[i].checked){
		  				t = document.w.elements[i].value;
		   			}
		   			break;
		   		case "c":
		   			if (document.w.elements[i].selectedIndex > 0){
			   			if (document.w.elements[i].options[document.w.elements[i].selectedIndex].selected){
			   				t = document.w.elements[i].options[document.w.elements[i].selectedIndex].value;
			   			}
			   		}
		   			break;
		   	}
  		}
 	}
 	if (t.length > 0) {
 		i=0;
 		while(i<t.length){
			if (t.substring(i,i+1) != ' ') {
		      return true;
			}
			i++;
		}
	}
 	return false;
}

//去除字串兩旁的空白
//trim(字串)
function trim(txt){
	var left, right;
	var txt2 = ""
	for(i=0; i<txt.length; i++){
		if(txt.charAt(i) == " "){
			continue;
		}else{
			left = i;
			break;
		}
	}
	for(i=txt.length-1; i>=0; i--){
		if(txt.charAt(i) == " "){
			continue;
		}else{
			right = i;
			break;
		}
	}
	for(i=left; i<=right; i++){
		txt2 = txt2 + txt.charAt(i);
	}
	return txt2;
}

//勾選或取消該名稱所有選項
//selectAll(輸入格名稱)
var mode=0;
function selectAll(txt) {
	var i =0;
	if (mode==0) mode=1; else mode=0;
	while (i < document.w.elements.length){
		if (document.w.elements[i].name == txt) {
			document.w.elements[i].checked=mode;
		}
		i++;
	}
}

//另開視窗
//windowopen(視窗名稱,連結網址,視窗寬度,視窗高度,是否有捲軸(0 or 1))
var windowstatus=0;
function windowopen(name,txt,w,h,s) {
	if (windowstatus>0) {
		windowobject.close();
	}
   	windowobject=window.open(txt,name,"scrollbars="+s+",width="+w+",height="+h+",resizable=1,toolbar=0,location=0,directories=0,status=0,menubar=0,titlebar=0,channelmode=0,fullscreen=0,left=0,top=0");
	windowstatus=1;
}

//檢查是否含不合法字元
//chkStr(字串)
function chkStr(str){
	var i,len,s,chk=true;
	var temp = "-_1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
	len = str.length;
   	test.innerHTML = "";
   	for (i=0; i<len; i++){
		if (!(temp.indexOf(str) >=0)){
			chk = false;
		}
    }
    test.innerHTML = chk;
}

//顯示或隱藏物件
function show(obj){
	if (obj!=""){
		if (eval(obj + ".className") =="expanded"){
			eval(obj + ".className='collapsed'");
		} else {
			eval(obj + ".className='expanded'");
		}
	}
}

//Macromedia JavaScript Funciton
function MM_preloadImages() { //v3.0
  var d=document; if(d.images){ if(!d.MM_p) d.MM_p=new Array();
    var i,j=d.MM_p.length,a=MM_preloadImages.arguments; for(i=0; i<a.length; i++)
    if (a[i].indexOf("#")!=0){ d.MM_p[j]=new Image; d.MM_p[j++].src=a[i];}}
}

function MM_preloadImages() { //v3.0
  var d=document; if(d.images){ if(!d.MM_p) d.MM_p=new Array();
    var i,j=d.MM_p.length,a=MM_preloadImages.arguments; for(i=0; i<a.length; i++)
    if (a[i].indexOf("#")!=0){ d.MM_p[j]=new Image; d.MM_p[j++].src=a[i];}}
}

function MM_findObj(n, d) { //v4.01
  var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {
    d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
  if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
  for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document);
  if(!x && d.getElementById) x=d.getElementById(n); return x;
}

function MM_swapImage() { //v3.0
  var i,j=0,x,a=MM_swapImage.arguments; document.MM_sr=new Array; for(i=0;i<(a.length-2);i+=3)
   if ((x=MM_findObj(a[i]))!=null){document.MM_sr[j++]=x; if(!x.oSrc) x.oSrc=x.src; x.src=a[i+2];}
}

function chgTDcolor(Obj,color){
	Obj.style.backgroundColor=color;
}