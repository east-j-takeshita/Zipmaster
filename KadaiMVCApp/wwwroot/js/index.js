Vue({
    el: '#page-demo',
    data: {
        page: 0,
        dispItemSize: 3,
        data: [
            { key: '1', value: 'value1' },
            { key: '2', value: 'value2' },
            { key: '3', value: 'value3' },
            { key: '4', value: 'value4' },
            { key: '5', value: 'value5' },
            { key: '6', value: 'value6' },
            { key: '7', value: 'value7' },
            { key: '8', value: 'value8' },
            { key: '9', value: 'value9' },
            { key: '10', value: 'value10' }
        ]
    },
    methods: {
        showFirst: function () {
            this.page = 0;
        },
        showPrev: function () {
            if (this.isStartPage) return;
            this.page--;
        },
        showNext: function () {
            if (this.isEndPage) return;
            this.page++;
        },
        showLast: function () {
            this.page = Math.floor(this.data.length / this.dispItemSize);
        },
        showPage: function (index) {
            this.page = index;
        }
    },
    computed: {
        dipsItems: function () {
            var startPage = this.page * this.dispItemSize;
            return this.data.slice(startPage, startPage + this.dispItemSize);
        },
        isStartPage: function () {
            return (this.page == 0);
        },
        isEndPage: function () {
            return ((this.page + 1) * this.dispItemSize >= this.data.length);
        },
        pageCount: function () {
            return Math.ceil(this.data.length / this.dispItemSize);
        }
    }
});