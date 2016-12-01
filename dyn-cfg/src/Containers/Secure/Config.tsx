import * as React from 'react';

import { connect } from 'react-redux'
import { Dispatch } from "redux";
import { StoreState } from '../../reducers'
import { Logout } from '../../actions'
import { push } from 'react-router-redux'


interface ConfigProps {
    dispatch: Dispatch<any>;
}

function mapStateToProps(state: StoreState) {
    return {
    };
}

class ConfigContainer extends React.Component<ConfigProps, any> {

    constructor() {
        super();
    }

    render() {

        return (
            <div>
                {this.props.children}
            </div>
        );
    }
}

export default connect(mapStateToProps)(ConfigContainer)

