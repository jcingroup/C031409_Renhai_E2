using System;
using System.Collections.Generic;

using ProcCore.NetExtension;

namespace ProcCore.CKEdit
{
    public enum EditBarNames
    {
        document, clipboard, editing, forms, basicstyles, paragraph, links, insert, styles, colors, tools
    }
    public enum EditFun
    {
        Dot_, Source, Save, NewPage, DocProps, Preview, Print, Templates,
        Cut, Copy, Paste, PasteText, PasteFromWord, Undo, Redo,
        Find, Replace, SelectAll, SpellChecker, Scayt,
        Form, Checkbox, Radio, TextField, Textarea, Select, Button, ImageButton, HiddenField,
        Bold, Italic, Underline, Strike, Subscript, Superscript, RemoveFormat,
        NumberedList, BulletedList, Outdent, Indent, Blockquote, CreateDiv, JustifyLeft, JustifyCenter, JustifyRight, JustifyBlock, BidiLtr, BidiRtl,
        Link, Unlink, Anchor,
        Image, Flash, Table, HorizontalRule, Smiley, SpecialChar, PageBreak, Iframe,
        Styles, Format, Font, FontSize,
        TextColor, BGColor,
        Maximize, ShowBlocks, About
    }
    public enum EditSkin { moono,office2003, v2, kama }
}
