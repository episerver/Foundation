const path = require("path");
const webpack = require("webpack");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const NodeSassGlobImporter = require('node-sass-glob-importer');

module.exports = {
    entry: {
        main: path.join(__dirname, 'Assets/js', 'main.js'),
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
                    {
                        loader: 'sass-loader',
                        options: {
                            sassOptions: {
                                importer: NodeSassGlobImporter()
                            }
                        }
                    },
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