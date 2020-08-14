const path = require("path");
const webpack = require("webpack");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

module.exports = {
    entry: {
        main: "./Assets/js/main.js"
    },
    resolve: {
        modules: [__dirname, "node_modules"],
    },
    output: {
        filename: "[name].min.js",
        path: path.resolve(__dirname, "Assets/js"),
    },
    plugins: [
        new webpack.ProvidePlugin({
            $: "jquery",
            jQuery: "jquery",
            "window.jQuery": "jquery",
            axios: "axios",
        }),
        new MiniCssExtractPlugin({
            filename: "../scss/[name].min.css",
        }),
    ],
    module: {
        rules: [
            {
                test: /\.s?css$/,
                use: [
                    {
                        loader: MiniCssExtractPlugin.loader,
                    },
                    "css-loader",
                    "sass-loader",
                ],
            },
            {
                test: /\.(jpe?g|png|gif|woff|woff2|eot|ttf|otf|svg)$/i,
                loader: "file-loader",
                options: {
                    name: '../vendors/[name].[ext]',
                }
            },
        ],
    },
};