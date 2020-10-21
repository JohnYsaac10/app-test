(function webpackUniversalModuleDefinition(root, factory) {
  if (typeof exports === 'object' && typeof module === 'object')
    module.exports = factory();
  else if (typeof define === 'function' && define.amd)
    define('BG', [], factory);
  else if (typeof exports === 'object') exports['BG'] = factory();
  else root['BG'] = factory();
})(window, function() {
  return /******/ (function(modules) {
    // webpackBootstrap
    /******/ // The module cache
    /******/ var installedModules = {}; // The require function
    /******/
    /******/ /******/ function __webpack_require__(moduleId) {
      /******/
      /******/ // Check if module is in cache
      /******/ if (installedModules[moduleId]) {
        /******/ return installedModules[moduleId].exports;
        /******/
      } // Create a new module (and put it into the cache)
      /******/ /******/ var module = (installedModules[moduleId] = {
        /******/ i: moduleId,
        /******/ l: false,
        /******/ exports: {}
        /******/
      }); // Execute the module function
      /******/
      /******/ /******/ modules[moduleId].call(
        module.exports,
        module,
        module.exports,
        __webpack_require__
      ); // Flag the module as loaded
      /******/
      /******/ /******/ module.l = true; // Return the exports of the module
      /******/
      /******/ /******/ return module.exports;
      /******/
    } // expose the modules object (__webpack_modules__)
    /******/
    /******/
    /******/ /******/ __webpack_require__.m = modules; // expose the module cache
    /******/
    /******/ /******/ __webpack_require__.c = installedModules; // define getter function for harmony exports
    /******/
    /******/ /******/ __webpack_require__.d = function(exports, name, getter) {
      /******/ if (!__webpack_require__.o(exports, name)) {
        /******/ Object.defineProperty(exports, name, {
          enumerable: true,
          get: getter
        });
        /******/
      }
      /******/
    }; // define __esModule on exports
    /******/
    /******/ /******/ __webpack_require__.r = function(exports) {
      /******/ if (typeof Symbol !== 'undefined' && Symbol.toStringTag) {
        /******/ Object.defineProperty(exports, Symbol.toStringTag, {
          value: 'Module'
        });
        /******/
      }
      /******/ Object.defineProperty(exports, '__esModule', { value: true });
      /******/
    }; // create a fake namespace object // mode & 1: value is a module id, require it // mode & 2: merge all properties of value into the ns // mode & 4: return value when already ns object // mode & 8|1: behave like require
    /******/
    /******/ /******/ /******/ /******/ /******/ /******/ __webpack_require__.t = function(
      value,
      mode
    ) {
      /******/ if (mode & 1) value = __webpack_require__(value);
      /******/ if (mode & 8) return value;
      /******/ if (
        mode & 4 &&
        typeof value === 'object' &&
        value &&
        value.__esModule
      )
        return value;
      /******/ var ns = Object.create(null);
      /******/ __webpack_require__.r(ns);
      /******/ Object.defineProperty(ns, 'default', {
        enumerable: true,
        value: value
      });
      /******/ if (mode & 2 && typeof value != 'string')
        for (var key in value)
          __webpack_require__.d(
            ns,
            key,
            function(key) {
              return value[key];
            }.bind(null, key)
          );
      /******/ return ns;
      /******/
    }; // getDefaultExport function for compatibility with non-harmony modules
    /******/
    /******/ /******/ __webpack_require__.n = function(module) {
      /******/ var getter =
        module && module.__esModule
          ? /******/ function getDefault() {
              return module['default'];
            }
          : /******/ function getModuleExports() {
              return module;
            };
      /******/ __webpack_require__.d(getter, 'a', getter);
      /******/ return getter;
      /******/
    }; // Object.prototype.hasOwnProperty.call
    /******/
    /******/ /******/ __webpack_require__.o = function(object, property) {
      return Object.prototype.hasOwnProperty.call(object, property);
    }; // __webpack_public_path__
    /******/
    /******/ /******/ __webpack_require__.p = ''; // Load entry module and return exports
    /******/
    /******/
    /******/ /******/ return __webpack_require__((__webpack_require__.s = 0));
    /******/
  })(
    /************************************************************************/
    /******/ [
      /* 0 */
      /***/ function(module, exports, __webpack_require__) {
        module.exports = __webpack_require__(7);

        /***/
      },
      /* 1 */
      /***/ function(module, exports, __webpack_require__) {
        // extracted by mini-css-extract-plugin
        /***/
      },
      /* 2 */
      /***/ function(module, exports) {
        (function() {
          var inputsNumbers = Array.prototype.slice.call(
            document.querySelectorAll('[data-limit]')
          );
          var parent = null;

          if (inputsNumbers != null) {
            inputsNumbers.forEach(function(input) {
              var filter = (valid = input.getAttribute('data-filter'));
              var limit = input.getAttribute('data-limit');

              if (limit > 0) {
                input.oninput = function(e) {
                  if (this.value.length > limit) {
                    this.value = this.value.slice(0, limit);
                  }
                };
              }

              if (filter == 'only-numbers') {
                input.onkeydown = function(e) {
                  if ((e.key >= 0 && e.key <= 9) || e.keyCode == 8) {
                  } else {
                    e.preventDefault();
                    this.value = this.value.slice(0, this.value.length);
                  }
                };

                input.addEventListener('mousewheel', function(e) {
                  e.preventDefault();
                });
              }
            });
          }
        })();

        /***/
      },
      /* 3 */
      /***/ function(module, exports) {
        (function() {
          window.onload = function() {
            var progressNodes = Array.prototype.slice.call(
              document.querySelectorAll('.bg-progress')
            );
            progressNodes.forEach(function(progress) {
              var percent = parseInt(progress.getAttribute('data-progress'));
              var value = progress.getAttribute('data-value');
              var node = document.createElement('div');
              node.innerHTML = '<span>' + value + '</span>';
              node.style.width = percent + '%';
              progress.appendChild(node);
            });
          };
        })();

        /***/
      },
      /* 4 */
      /***/ function(module, exports) {
        (function() {
          var dropdowns = Array.prototype.slice.call(
            document.querySelectorAll('.bg-dropdown')
          );

          function isIE() {
            ua = navigator.userAgent;
            var is_ie = ua.indexOf('MSIE ') > -1 || ua.indexOf('Trident/') > -1;
            return is_ie;
          }

          dropdowns.forEach(function(dropdown) {
            if (isIE()) {
              dropdown.classList.add('no-show-pseudo');
            }
          });
        })();

        /***/
      },
      /* 5 */
      /***/ function(module, exports) {
        var items = document.querySelectorAll('.bg-accordion-item');
        items = Array.prototype.slice.call(items);
        items.forEach(function(item) {
          return item.addEventListener('click', openAccordion);
        });

        function openAccordion() {
          this.classList.toggle('bg-open');
        }

        /***/
      },
      /* 6 */
      /***/ function(module, exports) {
        (function() {
          var codes = Array.prototype.slice.call(
            document.querySelectorAll('.bg-passcode .bg-button')
          );
          var textfield = document.querySelector(
            '.bg-passcode-field .bg-textbox'
          );
          codes.forEach(function(code) {
            code.addEventListener('click', registerNumber);
          });

          function registerNumber(e) {
            if (e.target.classList.contains('erase')) {
              deleteCodeInDisplay();
            } else {
              setCodeinDisplay();
            }
          }

          function setCodeinDisplay() {
            textfield.value += '*';
          }

          function deleteCodeInDisplay() {
            textfield.value = '';
          }
        })();

        /***/
      },
      /* 7 */
      /***/ function(module, __webpack_exports__, __webpack_require__) {
        'use strict';
        __webpack_require__.r(__webpack_exports__);

        // EXTERNAL MODULE: ./assets/styles/main.scss
        var main = __webpack_require__(1);

        // EXTERNAL MODULE: ./components/01-atoms/textbox/textbox.js
        var textbox = __webpack_require__(2);

        // EXTERNAL MODULE: ./components/01-atoms/progress/progress.js
        var progress = __webpack_require__(3);

        // EXTERNAL MODULE: ./components/01-atoms/dropdown/dropdown.js
        var dropdown = __webpack_require__(4);

        // EXTERNAL MODULE: ./components/02-molecules/accordion-item/accordion-item.js
        var accordion_item = __webpack_require__(5);

        // EXTERNAL MODULE: ./components/02-molecules/passcodefield/passcodefield.js
        var passcodefield = __webpack_require__(6);

        // CONCATENATED MODULE: ./components/02-molecules/modal/modal.js
        function _classCallCheck(instance, Constructor) {
          if (!(instance instanceof Constructor)) {
            throw new TypeError('Cannot call a class as a function');
          }
        }

        function _defineProperties(target, props) {
          for (var i = 0; i < props.length; i++) {
            var descriptor = props[i];
            descriptor.enumerable = descriptor.enumerable || false;
            descriptor.configurable = true;
            if ('value' in descriptor) descriptor.writable = true;
            Object.defineProperty(target, descriptor.key, descriptor);
          }
        }

        function _createClass(Constructor, protoProps, staticProps) {
          if (protoProps) _defineProperties(Constructor.prototype, protoProps);
          if (staticProps) _defineProperties(Constructor, staticProps);
          return Constructor;
        }

        var BgModal =
          /*#__PURE__*/
          (function() {
            function BgModal() {
              _classCallCheck(this, BgModal);

              this._declareTriggers();
            }

            _createClass(BgModal, [
              {
                key: 'init',
                value: function init() {
                  this._declareTriggers();
                }
              },
              {
                key: '_declareTriggers',
                value: function _declareTriggers() {
                  var triggerModal = Array.prototype.slice.call(
                    document.querySelectorAll('[bg-modal]')
                  );

                  if (triggerModal.length > 0) {
                    this._createOverlay();

                    var global = this;
                    triggerModal.forEach(function(trigger) {
                      trigger.addEventListener('click', function(e) {
                        e.preventDefault();
                        var id = this.getAttribute('bg-modal');
                        var modalId = id;
                        var modal = document.getElementById(modalId);
                        var overlay = document.querySelector('.bg-overlay');

                        if (modal.classList.contains('bg-show')) {
                          global._hideModal(modal, overlay);
                        } else {
                          global._showModal(modal, overlay);
                        }
                      });
                    });
                  }
                }
              },
              {
                key: '_createOverlay',
                value: function _createOverlay() {
                  var overlay = document.createElement('div');
                  overlay.classList.add('bg-overlay');
                  document.body.appendChild(overlay);
                  return overlay;
                }
              },
              {
                key: '_hideModal',
                value: function _hideModal(modal, overlay) {
                  modal.classList.remove('bg-show');
                  modal.classList.add('bg-close');
                  overlay.classList.remove('bg-show');
                }
              },
              {
                key: '_showModal',
                value: function _showModal(modal, overlay) {
                  modal.classList.remove('bg-close');
                  modal.classList.add('bg-show');
                  overlay.classList.add('bg-show');
                  overlay.addEventListener('click', function(e) {
                    if (!modal.dataset.static) {
                      overlay.classList.remove('bg-show');
                      modal.classList.remove('bg-show');
                    }
                  });
                }
              },
              {
                key: '_getOverlay',
                value: function _getOverlay() {
                  var overlay = document.querySelector('.bg-overlay');

                  if (overlay) {
                    return overlay;
                  } else {
                    return this._createOverlay();
                  }
                }
              },
              {
                key: 'showModal',
                value: function showModal(modalId) {
                  var modal = document.getElementById(modalId);

                  if (!modal) {
                    throw new Error('No hay Modal para abrir!');
                  }

                  var overlay = this._getOverlay();

                  this._showModal(modal, overlay);
                }
              },
              {
                key: 'hideModal',
                value: function hideModal(modalId) {
                  var modal = document.getElementById(modalId);

                  if (!modal) {
                    throw new Error('No hay Modal para remover!');
                  }

                  var overlay = document.querySelector('.bg-overlay');

                  this._hideModal(modal, overlay);
                }
              }
            ]);

            return BgModal;
          })();

        /* harmony default export */ var modal = BgModal;
        // CONCATENATED MODULE: ./components/01-atoms/otp/otp.js
        function otp_classCallCheck(instance, Constructor) {
          if (!(instance instanceof Constructor)) {
            throw new TypeError('Cannot call a class as a function');
          }
        }

        function otp_defineProperties(target, props) {
          for (var i = 0; i < props.length; i++) {
            var descriptor = props[i];
            descriptor.enumerable = descriptor.enumerable || false;
            descriptor.configurable = true;
            if ('value' in descriptor) descriptor.writable = true;
            Object.defineProperty(target, descriptor.key, descriptor);
          }
        }

        function otp_createClass(Constructor, protoProps, staticProps) {
          if (protoProps)
            otp_defineProperties(Constructor.prototype, protoProps);
          if (staticProps) otp_defineProperties(Constructor, staticProps);
          return Constructor;
        }

        var BgOtp =
          /*#__PURE__*/
          (function() {
            function BgOtp() {
              otp_classCallCheck(this, BgOtp);

              this.recordCode = [];
              this.otpContainer = document.querySelector('.bg-otp');
              this.secret = document.querySelector('.bg-otp [type=hidden]');

              if (this.otpContainer) {
                this.childs = Array.prototype.slice.call(
                  this.otpContainer.children
                );

                this._initOtp();
              }
            }

            otp_createClass(BgOtp, [
              {
                key: '_initOtp',
                value: function _initOtp() {
                  var _this = this;

                  this.childs.pop();
                  this.childs.forEach(function(child, index) {
                    return child.setAttribute('data-index', index);
                  });
                  this.childs.forEach(function(child) {
                    return child.addEventListener(
                      'keyup',
                      _this._changeBox.bind(_this)
                    );
                  });
                  this.childs.forEach(function(child) {
                    return child.addEventListener(
                      'keydown',
                      _this._cancelTab.bind(_this)
                    );
                  });
                  this.childs[0].focus();
                  this.otpContainer.addEventListener('click', function() {
                    if (_this.recordCode.length === 0) {
                      _this.childs[0].focus();
                    } else if (
                      _this.recordCode.length === _this.childs.length
                    ) {
                      _this.childs[_this.recordCode.length - 1].focus();
                    } else {
                      _this.childs[_this.recordCode.length].focus();
                    }
                  });
                }
              },
              {
                key: '_cancelTab',
                value: function _cancelTab(e) {
                  if (e.keyCode === 9) {
                    e.preventDefault();
                  }
                }
              },
              {
                key: 'reset',
                value: function reset() {
                  this.childs.forEach(function(child) {
                    child.value = '';
                  });
                  this.recordCode = [];
                  this.childs[0].focus();
                }
              },
              {
                key: '_changeBox',
                value: function _changeBox(e) {
                  if (e.keyCode === 8) {
                    this.recordCode.pop();
                    this.childs[this.recordCode.length].value = '';
                    this.childs[this.recordCode.length].focus();
                    this.secret.value = this.recordCode.join('');
                  } else {
                    var index = +e.target.dataset.index;

                    if (this.recordCode.length < this.childs.length) {
                      if (e.target.value !== '') {
                        //Send focus to the next input
                        if (this.childs[index + 1]) {
                          this.childs[index + 1].focus();
                        }

                        this.recordCode.push(e.target.value);
                      }
                    }

                    this.secret.value = this.recordCode.join('');
                  }
                }
              }
            ]);

            return BgOtp;
          })();

        /* harmony default export */ var otp = BgOtp;
        // CONCATENATED MODULE: ./assets/scripts/main.js

        if (typeof window !== 'undefined') {
          window.bgmodal = new modal();
          window.bgotp = new otp();
        }

        /***/
      }
      /******/
    ]
  );
});
