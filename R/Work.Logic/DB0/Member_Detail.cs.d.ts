declare module server {
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
	interface QueryBase {
		page: number;
	}
	interface snObject {
		y: number;
		m: number;
		d: number;
		w: number;
		sn_max: number;
	}
	interface BaseEntityTable {
		edit_type: number;
		check_del: boolean;
	}
	interface i_Code {
		code: string;
		langCode: string;
		value: string;
	}
	interface CUYUnit {
		sign: string;
		code: string;
	}
}
