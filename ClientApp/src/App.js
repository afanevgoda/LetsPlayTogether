import React, {Component} from 'react';
import {Route} from 'react-router';
import {Layout} from './components/Layout';
import {Home} from './components/Home';
import {FetchData} from './components/FetchData';
import {Counter} from './components/Counter';
import {Steam} from "./pages/steam/Steam";

import './custom.css'
import {createTheme} from "@mui/material/styles";
import {CssBaseline, ThemeProvider} from "@mui/material";
import {PollResult} from "./pages/steam/PollResult";

export default class App extends Component {
    static displayName = App.name;

    render() {

        const theme = createTheme({
            palette: {
                mode: 'dark',
                primary: {
                    light:'#60B5D7',
                    dark: '#60B5D7',
                    contrastText: '#60B5D7',
                    main: '#60B5D7',
                },
                good: {
                    main: '#8CBB56'
                },
                secondary: {
                    main: '#666768',
                },
                text:{
                    primary: '#60B5D7',
                    secondary: '#666768'
                },
                action: {
                    active: '#666768',
                    hover: '#666768',
                    selected: '#666768',
                    disabled: '#666768',
                    disabledBackground: '#666768'
                },
                background: {
                    default: '#1D1F24',
                    paper: '#1D1F24'
                }
            },
        });

        return (
            <ThemeProvider theme={theme}>
                <CssBaseline />
                <Layout>
                    <Route exact path='/' component={Home}/>
                    <Route path='/counter' component={Counter}/>
                    <Route path='/fetch-data' component={FetchData}/>
                    <Route path='/steam' component={Steam}/>
                    <Route path='/poll' component={PollResult}/>
                </Layout>
            </ThemeProvider>
        );
    }
}
