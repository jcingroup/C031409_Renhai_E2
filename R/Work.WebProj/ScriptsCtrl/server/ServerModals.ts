declare module server {

    interface BaseEntityTable {
        edit_type: number;
        check_del: boolean;
        expland_sub: boolean;
    }

    interface Member extends BaseEntityTable {
        member_id: number;
        householder: string;
        zip: string;
        address: string;
        tel: string;
        i_Hide: boolean;
        i_InsertUserID: string;
        i_InsertDeptID: number;
        i_InsertDateTime: Date;
        i_UpdateUserID: string;
        i_UpdateDeptID: number;
        i_UpdateDateTime: Date;
        i_Lang: string;
        member_Detail: any[];
    }
    interface Member_Detail extends BaseEntityTable {
        member_detail_id: number;
        member_id: number;
        is_holder: boolean;
        member_name: string;
        gender: boolean;
        birthday: Date;
        l_birthday: string;
        born_time: string;
        born_sign: string;
        tel: string;
        mobile: string;
        zip: string;
        address: string;
        email: string;
        i_Hide: boolean;
        i_InsertUserID: string;
        i_InsertDeptID: number;
        i_InsertDateTime: Date;
        i_UpdateUserID: string;
        i_UpdateDeptID: number;
        i_UpdateDateTime: Date;
        i_Lang: string;
        member: {
            getMemberDetail: server.Member_Detail[];
        };
    }
    interface Orders extends BaseEntityTable {
        orders_id: number;
        orders_sn: string;
        orders_state: string;
        transation_date: Date;
        member_id: number;
        member_detail_id: number;
        member_name: string;
        gender: boolean;
        tel: string;
        zip: string;
        address: string;
        mobile: string;
        email: string;
        pay_style: string;
        pay_date: Date;
        bank_serial: string;
        total: number;
        i_Hide: boolean;
        i_InsertUserID: string;
        i_InsertDeptID: number;
        i_InsertDateTime: Date;
        i_UpdateUserID: string;
        i_UpdateDeptID: number;
        i_UpdateDateTime: Date;
        i_Lang: string;
        orders_Detail: any[];
    }
    interface Orders_Detail extends BaseEntityTable {
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
        gender: boolean;
        l_birthday: string;
        born_time: string;
        born_sign: string;
        light_name: string;
        race: number;
        gold: number;
        manjushri: number;
        i_Hide: boolean;
        i_InsertUserID: string;
        i_InsertDeptID: number;
        i_InsertDateTime: Date;
        i_UpdateUserID: string;
        i_UpdateDeptID: number;
        i_UpdateDateTime: Date;
        i_Lang: string;
    }
} 