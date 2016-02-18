declare module server {
	interface Department extends BaseEntityTable {
		department_id: number;
		department_name: string;
		i_Hide: boolean;
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
