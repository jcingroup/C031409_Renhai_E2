module commData {
   export var born_sign: string[] = ['鼠', '牛', '虎', '兔', '龍', '蛇', '馬', '羊', '猴', '雞', '狗', '豬'];
   export var born_time: CBornTime[] = [
        { value: '吉', label: '00:00~23:59 吉時' },
        { value: '子', label: '23:00~01:00 子時' },
        { value: '丑', label: '01:00~03:00 丑時' },
        { value: '寅', label: '03:00~05:00 寅時' },
        { value: '卯', label: '05:00~07:00 卯時' },
        { value: '辰', label: '07:00~09:00 辰時' },
        { value: '巳', label: '09:00~11:00 巳時' },
        { value: '午', label: '11:00~13:00 午時' },
        { value: '未', label: '13:00~15:00 未時' },
        { value: '申', label: '15:00~17:00 申時' },
        { value: '酉', label: '17:00~19:00 酉時' },
        { value: '戌', label: '19:00~21:00 戌時' },
        { value: '亥', label: '21:00~23:00 亥時' }
    ];
}