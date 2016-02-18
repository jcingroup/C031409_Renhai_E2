interface IResultBase {
    result: boolean;
    message: string;
    data: any;
    id: number;
}

interface IResultData<T> {
    result: boolean;
    message: string;
    data: T;
}

interface IJSONBase {
    result: boolean;
    message: string;
    json: {};
}

interface IJSONData<T> {
    result: boolean;
    message: string;
    json: T;
}

interface IKeyValueS {
    value: string;
    label: string;
}

interface IKeyValue {
    value: number;
    label: string;
}