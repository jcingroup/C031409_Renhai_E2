function inherits(subConstructor, superConstructor) {
    var proto = Object.create(
        superConstructor.prototype,
        {
            "constructor": { 
                configurable: true,
                enumerable: false,
                writable: true,
                value: subConstructor
            }
        }
    );
    Object.defineProperty(subConstructor, "prototype",  { 
        configurable: true,
        enumerable: false,
        writable: true,
        value: proto
    });
}

var server = server || {};
/// <summary>The FilesUpScope class as defined in ProcCore.WebCore.FilesUpScope</summary>
server.FilesUpScope = function() {
	/// <field name="limitSize" type="Number">The LimitSize property as defined in ProcCore.WebCore.FilesUpScope</field>
	this.limitSize = 0;
	/// <field name="limitExtType" type="String[]">以副檔名為設定，此為禁止上傳類型，要加. 例.exe。</field>
	this.limitExtType = [];
	/// <field name="allowExtType" type="String[]">The AllowExtType property as defined in ProcCore.WebCore.FilesUpScope</field>
	this.allowExtType = [];
	/// <field name="limitCount" type="Number">The LimitCount property as defined in ProcCore.WebCore.FilesUpScope</field>
	this.limitCount = 0;
};

/// <summary>The ImageUpScope class as defined in ProcCore.WebCore.ImageUpScope</summary>
server.ImageUpScope = function() {
	/// <field name="kindName" type="String">The KindName property as defined in ProcCore.WebCore.ImageUpScope</field>
	this.kindName = '';
	/// <field name="keepOriginImage" type="Boolean">The KeepOriginImage property as defined in ProcCore.WebCore.ImageUpScope</field>
	this.keepOriginImage = false;
	/// <field name="parm" type="Object[]">The Parm property as defined in ProcCore.WebCore.ImageUpScope</field>
	this.parm = [];
};

inherits(ImageUpScope, FilesUpScope);/// <summary>The ImageSizeParm class as defined in ProcCore.WebCore.ImageSizeParm</summary>
server.ImageSizeParm = function() {
	/// <field name="sizeFolder" type="Number">The SizeFolder property as defined in ProcCore.WebCore.ImageSizeParm</field>
	this.sizeFolder = 0;
	/// <field name="heigh" type="Number">The heigh property as defined in ProcCore.WebCore.ImageSizeParm</field>
	this.heigh = 0;
	/// <field name="width" type="Number">The width property as defined in ProcCore.WebCore.ImageSizeParm</field>
	this.width = 0;
};

/// <summary>The ResultInfo class as defined in ProcCore.HandleResult.ResultInfo</summary>
server.ResultInfo = function() {
	/// <field name="result" type="Boolean">The result property as defined in ProcCore.HandleResult.ResultInfo</field>
	this.result = false;
	/// <field name="message" type="String">The message property as defined in ProcCore.HandleResult.ResultInfo</field>
	this.message = '';
	/// <field name="id" type="Number">The id property as defined in ProcCore.HandleResult.ResultInfo</field>
	this.id = 0;
	/// <field name="aspnetid" type="String">The aspnetid property as defined in ProcCore.HandleResult.ResultInfo</field>
	this.aspnetid = '';
	/// <field name="sessionout" type="Boolean">The sessionout property as defined in ProcCore.HandleResult.ResultInfo</field>
	this.sessionout = false;
	/// <field name="json" type="Object">The json property as defined in ProcCore.HandleResult.ResultInfo</field>
	this.json = { };
};

/// <summary>The ReturnAjaxFiles class as defined in ProcCore.HandleResult.ReturnAjaxFiles</summary>
server.ReturnAjaxFiles = function() {
	/// <field name="filesObject" type="Object[]">The filesObject property as defined in ProcCore.HandleResult.ReturnAjaxFiles</field>
	this.filesObject = [];
	/// <field name="fileName" type="String">The FileName property as defined in ProcCore.HandleResult.ReturnAjaxFiles</field>
	this.fileName = '';
	/// <field name="success" type="Boolean">搭配Fine Uploader</field>
	this.success = false;
	/// <field name="error" type="String">搭配Fine Uploader</field>
	this.error = '';
};

inherits(ReturnAjaxFiles, ResultInfo);/// <summary>The FilesObject class as defined in ProcCore.HandleResult.FilesObject</summary>
server.FilesObject = function() {
	/// <field name="representFilePath" type="String">The RepresentFilePath property as defined in ProcCore.HandleResult.FilesObject</field>
	this.representFilePath = '';
	/// <field name="originFilePath" type="String">The OriginFilePath property as defined in ProcCore.HandleResult.FilesObject</field>
	this.originFilePath = '';
	/// <field name="fileName" type="String">The FileName property as defined in ProcCore.HandleResult.FilesObject</field>
	this.fileName = '';
	/// <field name="filesKind" type="String">The FilesKind property as defined in ProcCore.HandleResult.FilesObject</field>
	this.filesKind = '';
	/// <field name="isImage" type="Boolean">The IsImage property as defined in ProcCore.HandleResult.FilesObject</field>
	this.isImage = false;
	/// <field name="size" type="Number">The Size property as defined in ProcCore.HandleResult.FilesObject</field>
	this.size = 0;
};

