declare module server {
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
