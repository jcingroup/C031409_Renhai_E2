declare module server {
	interface c_ShoppingMaster {
		orders_id: string;
		member_name: string;
		address: string;
		zip: string;
		detail: server.c_ShoppingDetail[];
	}
	interface c_ShoppingDetail {
		product_sn: string;
		product_name: string;
		member_detail_id: number;
		member_name: number;
		price: number;
		address: string;
		gender: boolean;
		l_birthday: string;
		born_time: string;
		born_sign: string;
	}
}
