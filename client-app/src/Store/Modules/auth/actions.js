// src/Store/Modules/auth/actions.js
import axios from 'axios'
import { SIGNIN_ACTIONS, SIGNUP_ACTIONS } from '../storeConstants.js'

export default {
  async [SIGNUP_ACTIONS]({ }, payload) {
    await axios.post(
      'http://localhost:5146/auth/signup',
      {
        username: payload.username,
        password: payload.password
      },
      { withCredentials: true }
    );
  },

  async [SIGNIN_ACTIONS]({ commit }, payload) {
    const response = await axios.post('http://localhost:5146/auth/signin', {
      username: payload.username,
      password: payload.password
    });
    const token = response.data.token;
    axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    commit('SET_TOKEN', token);
    localStorage.setItem('token', token);
    axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
  }
};
