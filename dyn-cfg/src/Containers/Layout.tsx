import * as React from 'react';
import { connect } from 'react-redux'

import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
import darkBaseTheme from 'material-ui/styles/baseThemes/darkBaseTheme';
import getMuiTheme from 'material-ui/styles/getMuiTheme';
import CircularProgress from 'material-ui/CircularProgress';

import { StoreState } from '../reducers'

interface LayoutProps {
    showLoading: boolean;
}

function mapStateToProps(state: StoreState): LayoutProps {
    return {
        showLoading: state.layout.asyncCount > 0
    };
}

const innerLoaderContainer = {
    position: 'absolute',
    top: '40%',
    left: 0,
    right: 0,
    zIndex: 10000
};

const loaderOverlay = {
    position: 'absolute',
    top: 0,
    bottom: 0,
    left: 0,
    right: 0,
    backgroundColor: 'black',
    zIndex: 9999,
    opacity: 0.2
}

class LayoutContainer extends React.Component<LayoutProps, any>{

    render() {
        return (
            <div style={{ textAlign: "center" }}>
                <MuiThemeProvider muiTheme= { getMuiTheme(darkBaseTheme) } >
                    <div>
                        <div style={{ display: this.props.showLoading ? 'block' : 'none' }}>
                            <div  style={ innerLoaderContainer }>
                                <CircularProgress mode="indeterminate" />
                            </div>
                            <div style= {loaderOverlay}></div>
                        </div>
                        {this.props.children}
                    </div>
                </MuiThemeProvider >
            </div >
        );
    }

}


export default connect(mapStateToProps)(LayoutContainer)