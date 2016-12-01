const webpack = require('webpack');
const path = require('path');
const buildPath = path.resolve(__dirname, 'build');
const TransferWebpackPlugin = require('transfer-webpack-plugin');

const config = {
    // Entry points to the project
    entry: [
        'webpack/hot/dev-server',
        'webpack/hot/only-dev-server',
        path.join(__dirname, '/src/app.tsx')
    ],
    output: {
        path: buildPath, // Path of output file
        filename: 'app.js',
    },
    // Server Configuration options
    devServer: {
        contentBase: 'src', // Relative directory for base of server
        devtool: 'eval',
        hot: true, // Live-reload
        inline: true,
        port: 4001, // Port Number
        host: 'localhost', // Change to '0.0.0.0' for external facing server
    },
    devtool: 'eval',
    plugins: [
        // Enables Hot Modules Replacement
        new webpack.HotModuleReplacementPlugin(),
        // Allows error warnings but does not stop compiling.
        new webpack.NoErrorsPlugin(),
        // Moves files
        new TransferWebpackPlugin([
            { from: '' },
        ], path.resolve(__dirname, 'src')),
        new webpack.PrefetchPlugin("react")
    ],
    module: {
        loaders: [
            {
                test: /\.tsx?$/,
                exclude: /(node_modules)/,
                loaders: ['react-hot-loader/webpack', 'ts-loader']
            }
        ]
    },
    resolve: {
        extensions: ["", ".webpack.js", ".web.js", ".ts", ".tsx", ".js", ".css"]
    },
};

module.exports = config;
