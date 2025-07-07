import Validations from './Validations';

export default class signUpValidation {
  constructor(email, password) {
    this.email = email;
    this.password = password;
  }

  checkValidations() {
    let errors = {}; // ✅ объект, не массив

    // email validation
    if (!Validations.checkEmail(this.email)) {
      errors['email'] = 'Invalid email';
    }

    // password validation
    if (!Validations.minLength(this.password, 6)) {
      errors['password'] = 'Password should be at least 6 characters';
    }

    return errors;
  }
}
