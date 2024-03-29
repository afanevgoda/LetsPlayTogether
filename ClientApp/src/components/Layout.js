import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';
import styles from './layout.module.css';

export class Layout extends Component {
  static displayName = Layout.name;

  render () {
    return (
      <div>
        <NavMenu />
        <Container className={styles.container}>
          {this.props.children}
        </Container>
      </div>
    );
  }
}
