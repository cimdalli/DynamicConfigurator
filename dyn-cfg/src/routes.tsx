import * as React from 'react';
import { Route, Redirect } from 'react-router'

import { authChecker } from './utils/routeHelpers'

import Layout from './containers/Layout'
import Login from './containers/Public/Login'
import Main from './containers/Secure/Main'
import Config from './containers/Secure/Config'

export default
    <Route component={Layout} onChange={authChecker('login', 'config', 'auth') } >
        <Route path="login" component={Login} />
        <Route component={Main} >
            <Route path="config" component={Config} />
        </Route>
        <Redirect from="*" to='config'/>
    </Route>
