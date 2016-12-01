import * as React from 'react';

import { connect } from 'react-redux'
import { Dispatch } from "redux";
import { StoreState } from '../../reducers'
import { Logout } from '../../actions'
import { push } from 'react-router-redux'


interface MainProps {
    dispatch: Dispatch<any>;
    useDarkTheme: boolean;
}

function mapStateToProps(state: StoreState) {
    return {
    };
}

class MainContainer extends React.Component<MainProps, any> {

    constructor() {
        super();

        this.logout = this.logout.bind(this);
    }


    logout() {
        this.props.dispatch(new Logout());
    }

    render() {

        return (
            <div>
                {this.props.children}
            </div>
        );
    }
}

export default connect(mapStateToProps)(MainContainer)

