const fs = require("fs");
const path = require("path");

const prettierOptions = JSON.parse(fs.readFileSync(path.resolve(__dirname, ".prettierrc.json"), "utf8"));

module.exports = {
	env: {
		browser: true,
		es2021: true,
	},
	overrides: [
		{
			files: ["**/*.ts?(x)"],
			rules: {
				"@typescript-eslint/explicit-function-return-type": "off",
				"max-lines": "off",
				"max-lines-per-function": "off",
				"no-magic-numbers": "off",
				"react/jsx-key": "off",
				"react/jsx-props-no-spreading": "off",
				"no-undef": "off",
			},
		},
	],
	extends: ["plugin:react/recommended", "airbnb", "prettier"],
	parser: "@typescript-eslint/parser",
	parserOptions: {
		ecmaFeatures: {
			jsx: true,
		},
		ecmaVersion: "latest",
		sourceType: "module",
	},
	plugins: ["prettier", "react", "@typescript-eslint"],
	settings: {
		"import/resolver": {
			node: {
				extensions: [".js", ".jsx", ".ts", ".tsx"],
				moduleDirectory: ["src", "node_modules"],
			},
		},
	},
	rules: {
		"@typescript-eslint/no-non-null-assertion": "error",
		"react/react-in-jsx-scope": "off",
		"quotes": [2, "double", { avoidEscape: true }],
		"react/function-component-definition": [2, { namedComponents: "arrow-function" }],
		"prettier/prettier": ["warn", prettierOptions],
		"@typescript-eslint/no-explicit-any": "off",
		"react-hooks/exhaustive-deps": "off",
		"no-unused-vars": "off",
		"linebreak-style": "off",
		"max-len": ["error", { code: 120 }],
		"no-use-before-define": "off",
		"import/prefer-default-export": "0",
		"import/no-default-export": "error",
		"@typescript-eslint/no-use-before-define": ["error"],
		"arrow-parens": "off",
		"object-curly-newline": "off",
		"import/no-unresolved": "off",
		"react/require-default-props": "off",
		"indent": "off",
		"react/jsx-wrap-multilines": "off",
		"implicit-arrow-linebreak": "off",
		"@typescript-eslint/no-empty-interface": "off",
		"no-spaced-func": "off",
		"func-call-spacing": "off",
		"no-shadow": "off",
		"react/prop-types": "off",
		"@typescript-eslint/no-shadow": "off",
		"import/no-extraneous-dependencies": ["error", { devDependencies: true }],
		"react/jsx-filename-extension": [1, { extensions: [".tsx", ".ts"] }],
		"no-loop-func": "off",
		"import/extensions": [
			"error",
			"ignorePackages",
			{
				js: "never",
				jsx: "never",
				ts: "never",
				tsx: "never",
			},
		],
		"import/order": [
			"warn",
			{
				"groups": [["builtin", "external"], "internal", ["parent", "sibling", "index"], "object", "type"],
				"newlines-between": "always",
				"alphabetize": {
					order: "asc",
				},
				"pathGroups": [
					{
						pattern: "./**/*.less",
						group: "object",
					},
					{
						pattern: "**/*.less",
						group: "object",
					},
					{
						pattern: "./**/*.{jpg,jpeg,png,gif,svg,ico}",
						group: "type",
					},
					{
						pattern: "**/*.{jpg,jpeg,png,gif,svg,ico}",
						group: "type",
					},
				],
			},
		],
	},
};
