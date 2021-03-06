﻿declare enum IEditType {
    none = 0,
    insert = 1,
    update = 2
}

declare enum e_orders_type {
    general = 0,
    mdlight = 1, //主副斗
    sdlight = 2, //大中小斗
    fortune_order = 3,//福燈
    wishlight = 4,//祈福許願燈
    doulight = 5 //媽祖/藥師佛斗燈
}

declare enum e_orders_state {
    waiting = 0,
    handling = 1,
    complete = 2,
    reject = 3,
    invalid = 4
}