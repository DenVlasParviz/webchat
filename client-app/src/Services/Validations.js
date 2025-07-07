export default class Validations {
  static minLength(str, minLength) {
    return str.length >= minLength;
  }

  static checkEmail(email) {
    const re = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
    return re.test(email);
  }
}
