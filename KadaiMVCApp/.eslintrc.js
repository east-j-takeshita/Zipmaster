// .eslintrc.js
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
        ecmaVersion: 2021,
        parser: '@babel/eslint-parser',
    },
    rules: {
        // 必要に応じてルールを追加
    },
};
