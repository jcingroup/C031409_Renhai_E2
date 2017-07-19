declare module server {
    interface BaseEntityTable {
        edit_type: number;
        check_del: boolean;
        expland_sub: boolean;
    }
    interface Orders {
        orders_id: number;
        orders_sn: string;
        y: number;
        member_detail_id: number;
        member_name: string;
        tel: string;
        zip: string;
        address: string;
        mobile: string;
        gender: string;
        申請人生日: string;
        email: string;
        total: number;
        付款方式: string;
        transation_date: Date;
        付款時間: Date;
        查詢序號: string;
        付款方式名稱: string;
        訂單狀態名稱: string;
        銀行帳號: string;
        新增時間: Date;
        InsertUserId: number;
        member_id: number;
        orders_type: number;
        orders_state: number;
        c_Hiden: boolean;
        c_InsertUserID: number;
        c_InsertDateTime: Date;
        c_UpdateUserID: number;
        c_UpdateDateTime: Date;
        orders_Detail: any[];
    }
    interface Orders_Detail {
        orders_detail_id: number;
        orders_sn: string;
        product_sn: string;
        member_detail_id: number;
        y: number;
        product_name: string;
        price: number;
        amt: number;
        member_name: string;
        address: string;
        gender: string;
        l_birthday: string;
        age: string;
        born_time: string;
        born_sign: string;
        購買時間: Date;
        付款時間: Date;
        memo: string;
        zip: string;
        light_name: string;
        經手人: string;
        i_InsertDateTime: Date;
        i_InsertUserID: number;
        race: number;
        gold: number;
        tran_mark: boolean;
        manjushri: number;
        detail_sort: number;
        c_Hiden: boolean;
        c_InsertUserID: number;
        c_InsertDateTime: Date;
        c_UpdateUserID: number;
        c_UpdateDateTime: Date;
        orders: server.Orders;
        product: server.Product;
    }
    interface Product extends server.BaseEntityTable{
        product_sn: string;
        product_name: string;
        category: string;
        isSelect: boolean;
        isLight: boolean;
        price: number;
        i_Hide: number;
        排序: number;
        c_InsertUserID: number;
        c_InsertDateTime: Date;
        c_UpdateUserID: number;
        c_UpdateDateTime: Date;
        orders_Detail: server.Orders_Detail[];
    }
    interface Member {
        member_id: number;
        tel: string;
        zip: string;
        address: string;
        householder: string;
        i_InsertDateTime: Date;
        repeat_mark: boolean;
        member_Detail: server.Member_Detail[];
    }
    interface Member_Detail {
        member_detail_id: number;
        member_id: number;
        is_holder: boolean;
        member_name: string;
        電話區碼: string;
        tel: string;
        zip: string;
        address: string;
        mobile: string;
        gender: string;
        lbirthday: string;
        born_time: string;
        email: string;
        Memo: string;
        born_sign: string;
        city: string;
        country: string;
        i_InsertDateTime: Date;
        member: server.Member;
    }
    interface Light_Site {
        light_site_id: number;
        light_name: string;
        y: number;
        is_sellout: string;
        product_sn: string;
        price: number;
        IsReject: boolean;
    }
    interface cartMaster {
        member_id: number;
        member_detail_id: number;
        member_name: string;
        gender: string;
        zip: string;
        address: string;
        mobile: string;
        tel: string;
        total: number;
        race: number;
        gold: number;
        is_light_serial: boolean;
        Item: server.cartDetail[];
        orders_sn: string;
        transation_date: Date;
        y?: number;
    }
    interface cartDetail {
        price: number;
        product_sn: string;
        /** 可選擇燈位需用到 主副斗用 */
        light_site_id: number;
        light_name: string;
        tel: string;
        mobile: string;
        product_name: string;
        /** 這裡指的是會員資料表中的序號 */
        member_detail_id: number;
        member_name: string;
        address: string;
        gender: string;
        born_time: string;
        born_sign: string;
        l_birthday: string;
        age: string;
        /** 析福事項 */
        memo: string;
        race: number;
        gold: number;
        manjushri: number;
        /**契子入會日期*/
        join_date: string;

        SY: number;
        SM: number;
        SD: number;

        LY: number;
        LM: number;
        LD: number;
        isOnLeapMonth: boolean;
        isOnOrder: boolean;

        y: number;
    }
    interface LuniInfo {
        SY: number;
        /** 西元 */
        LY: number;
        /** 民國 */
        M: number;
        D: number;
        IsLeap: boolean;
    }
    interface Users {
        users_id: number;
        account: string;
        密碼: string;
        users_name: string;
        住址: string;
        家裡電話: string;
        手機: string;
        生日: Date;
        units_id: number;
        停權: number;
        職稱代碼: number;
        抬頭稱呼: string;
        電子信箱: string;
        isadmin: number;
        使用IP: string;
        mD5: string;
        訂單主檔: any[];
    }
    interface Reject {
        reject_id: number;
        reject_date: Date;
        user_id: number;
        total: number;
        orders_sn: string;
        reject_Detail: any[];
        y: number;
        orders: server.Orders;
    }
    interface Reject_Detail {
        reject_detail_id: number;
        reject_id: number;
        y: number;
        light_site_id: number;
        light_name: string;
        price: number;
        member_detail_id: number;
        orders_detail_id: number;
        reject: server.Reject;
    }
    interface Fortune_Light {
        fortune_light_id: number;
        order_sn: string;
        y: number;
        member_detail_id: number;
        member_name: string;
        memo: string;
        sort: number;
    }
    interface Sort_Member {
        member_detail_id: number;
        member_name: string;
        sort: number;
    }
    interface TempleMember extends server.BaseEntityTable {
        temple_member_id: number;
        sno: string;
        member_name: string;
        zip: string;
        addr: string;
        birthday: string;
        tel: string;
        mobile: string;
        i_insertDateTime: Date;
        join_datetime: Date;
        is_close: boolean;
        memo: string;
        temple_Account: any[];
    }
    interface TempleAccount {
        temple_account_id: number;
        product_sn: string;
        tran_date: Date;
        price: number;
        temple_member_id: number;
        templeMember: server.TempleMember;
    }
    interface Manjushri {
        manjushri_id: number;
        stage: string;
        set_date: Date;
        lY: string;
        lM: string;
        lD: string;
        people: number;
    }

    interface AssemblyBatch extends server.BaseEntityTable {
        batch_sn: number;
        batch_title: string;
        batch_date: any;
        lunar_y: any;
        lunar_m: any;
        lunar_d: any;
        batch_qty: number;
        //擴充
        count: number;
    }
}

//下面為擴充屬性
declare module server {
    interface Orders {
        getOrders_Detail: server.Orders_Detail[];
    }
    interface Member {
        getMember_Detail: server.Member_Detail[];
    }
    interface Member_Detail {
        fortune_value: number;
        sort: number;//福燈排序
        join_date: string;//契子入會日期
    }
}
