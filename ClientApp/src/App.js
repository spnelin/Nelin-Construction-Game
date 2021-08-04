import React, { Component } from 'react';
import { SiteCore } from './components/SiteCore';

export default class App extends Component {
  displayName = App.name

  render() {
    return (
      <SiteCore />
    );
  }
}
