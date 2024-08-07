
module.exports = {
    root: true,
    env: {
        browser: true,
        es2021: true,
    },
    extends: [
        'plugin:vue/vue3-recommended',
        'eslint:recommended',
    ],
    parserOptions: {
        parser: 'babel-eslint',
    },
    rules: {
        // 必要に応じてルールをカスタマイズ
    },
};